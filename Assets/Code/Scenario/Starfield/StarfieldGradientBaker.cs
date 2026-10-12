using UnityEngine;

public static class StarfieldGradientBaker
{
    public static Texture2D Bake(
        StarfieldConfig config,
        int resolution = 256)
    {
        if (config == null || config.layers == null ||
            config.layers.Count == 0)
        {
            return null;
        }

        resolution = Mathf.Max(2, resolution);

        int layerCount = config.layers.Count;

        var texture = new Texture2D(
            resolution,
            layerCount,
            TextureFormat.RGBAHalf,
            false,
            true
        )
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear,
            name = "Starfield Gradient Atlas"
        };

        for (int layerIndex = 0;
             layerIndex < layerCount;
             layerIndex++)
        {
            Gradient gradient =
                config.layers[layerIndex].colorGradient
                ?? new Gradient();

            for (int x = 0; x < resolution; x++)
            {
                float t = x / (float)(resolution - 1);

                Color color = gradient.Evaluate(t);

                texture.SetPixel(x, layerIndex, color);
            }
        }

        texture.Apply(false, true);

        return texture;
    }
}
