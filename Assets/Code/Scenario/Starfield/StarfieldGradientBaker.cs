using System.Collections.Generic;
using Code.Scenario.Starfield;
using UnityEngine;

/// <summary>
/// Bakes every color the background shader needs into one atlas: one row
/// per star layer gradient, then one row per nebula palette.
/// </summary>
public static class StarfieldGradientBaker
{
    /// <param name="starConfig">Its layers fill rows 0 to N-1.</param>
    /// <param name="nebulaPalettes">
    /// Fill the rows after the star layers, in order. A null entry leaves
    /// its row black.
    /// </param>
    public static Texture2D Bake(
        StarfieldConfig starConfig,
        IReadOnlyList<NebulaPalette> nebulaPalettes,
        int resolution = 256)
    {
        int starRows = starConfig != null && starConfig.layers != null
            ? starConfig.layers.Count
            : 0;

        int nebulaRows = nebulaPalettes?.Count ?? 0;
        int rows = starRows + nebulaRows;

        if (rows == 0)
            return null;

        resolution = Mathf.Max(2, resolution);

        var texture = new Texture2D(
            resolution,
            rows,
            TextureFormat.RGBAHalf,
            false,
            true
        )
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear,
            name = "Starfield Gradient Atlas"
        };

        for (var layerIndex = 0; layerIndex < starRows; layerIndex++)
        {
            Gradient gradient =
                starConfig.layers[layerIndex]?.colorGradient ?? new Gradient();

            for (var x = 0; x < resolution; x++)
            {
                float t = x / (float)(resolution - 1);

                texture.SetPixel(x, layerIndex, gradient.Evaluate(t));
            }
        }

        // The texture is linear, so in a linear color space project the
        // palette is converted on the way in: the pixel on screen is then
        // exactly the hex the designer picked.
        bool linear = QualitySettings.activeColorSpace == ColorSpace.Linear;

        for (var i = 0; i < nebulaRows; i++)
            BakePalette(texture, starRows + i, nebulaPalettes[i], resolution, linear);

        texture.Apply(false, true);

        return texture;
    }

    /// <summary>
    /// Splits the row into equal segments, one flat color per tone, so
    /// sampling the centre of a segment returns that tone exactly even
    /// with bilinear filtering.
    /// </summary>
    private static void BakePalette(
        Texture2D texture,
        int row,
        NebulaPalette palette,
        int resolution,
        bool linear)
    {
        int tones = palette != null ? palette.ToneCount : 0;

        for (var x = 0; x < resolution; x++)
        {
            Color color = Color.black;

            if (tones > 0)
            {
                int tone = Mathf.Min(tones - 1, x * tones / resolution);
                color = palette.Tones[tone];

                if (linear)
                    color = color.linear;
            }

            texture.SetPixel(x, row, color);
        }
    }
}
