using System.Collections.Generic;
using UnityEngine;

namespace Code.Scenario.Starfield
{
    /// <summary>
    /// Discrete colors a nebula layer is drawn with, from its faintest to
    /// its densest tone.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Colors are used exactly as picked: the shader selects one of them
    /// per sprite pixel and never blends between them, so the result
    /// stays on the palette like hand-made pixel art.
    /// </para>
    /// <para>
    /// A palette is an asset of its own so several nebula layers, or
    /// several sectors of the map, can share it.
    /// </para>
    /// </remarks>
    [CreateAssetMenu(
        fileName = "NebulaPalette",
        menuName = "The Invasion/Nebula Palette"
    )]
    public class NebulaPalette : ScriptableObject
    {
        /// <summary>
        /// Most colors a palette can have. Must stay below the atlas
        /// width divided by two so every tone keeps whole texels.
        /// </summary>
        public const int MaxTones = 6;

        [Tooltip(
            "Colors from the faintest to the densest tone. Empty space " +
            "is not part of the palette."
        )]
        [SerializeField]
        private List<Color> tones = new()
        {
            new Color32(22, 14, 18, 255),
            new Color32(38, 20, 28, 255),
            new Color32(58, 28, 40, 255),
            new Color32(82, 40, 52, 255)
        };

        [Tooltip(
            "Relative luminance above which the inspector warns that a " +
            "tone may compete with ships and projectiles."
        )]
        [SerializeField]
        [Range(0f, 1f)]
        private float luminanceWarning = 0.12f;

        public IReadOnlyList<Color> Tones => tones;

        /// <summary>
        /// Colors actually used, at most <see cref="MaxTones"/>.
        /// </summary>
        public int ToneCount => Mathf.Min(tones.Count, MaxTones);

        public float LuminanceWarning => luminanceWarning;

        /// <summary>
        /// Relative luminance of the brightest tone, in [0, 1].
        /// </summary>
        public float MaxLuminance
        {
            get
            {
                var max = 0f;

                for (var i = 0; i < ToneCount; i++)
                    max = Mathf.Max(max, RelativeLuminance(tones[i]));

                return max;
            }
        }

        /// <summary>
        /// Replaces the tones with a list of hex colors, as exported by
        /// Aseprite or Lospec: "#222034, #3f3f74" or one "222034" per line.
        /// </summary>
        /// <returns>False, leaving the palette untouched, when a value
        /// cannot be read.</returns>
        public bool TryImportHex(string text, out string error)
        {
            var parsed = new List<Color>();

            string[] values = text.Split(
                new[] { ',', ';', ' ', '\n', '\r', '\t' },
                System.StringSplitOptions.RemoveEmptyEntries
            );

            foreach (string value in values)
            {
                string hex = value.StartsWith("#") ? value : "#" + value;

                if (!ColorUtility.TryParseHtmlString(hex, out Color color))
                {
                    error = $"'{value}' is not a hex color.";
                    return false;
                }

                parsed.Add(color);
            }

            if (parsed.Count == 0 || parsed.Count > MaxTones)
            {
                error = $"A palette needs between 1 and {MaxTones} colors; " +
                        $"found {parsed.Count}.";
                return false;
            }

            tones = parsed;
            error = null;

            SpaceBackgroundAssets.NotifyChanged();
            return true;
        }

        /// <summary>
        /// WCAG relative luminance of a color picked in sRGB.
        /// </summary>
        public static float RelativeLuminance(Color color)
        {
            Color linear = color.linear;

            return 0.2126f * linear.r + 0.7152f * linear.g + 0.0722f * linear.b;
        }

        private void OnValidate()
        {
            SpaceBackgroundAssets.NotifyChanged();
        }
    }
}
