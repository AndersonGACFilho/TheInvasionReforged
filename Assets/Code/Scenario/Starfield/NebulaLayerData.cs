using UnityEngine;

namespace Code.Scenario.Starfield
{
    /// <summary>
    /// Packs nebula layers into the float4 uniform array read by
    /// StarfieldNebula.hlsl.
    /// </summary>
    /// <remarks>
    /// Layout per layer, matching LoadNebulaLayer in the shader:
    /// <code>
    /// [0] enabled,          kind,              parallaxFactor,   intensity
    /// [1] frequency,        octaves,           warp,             threshold
    /// [2] contrast,         veil,              flowRadius,       toneCount
    /// [3] regionMin,        regionMax,         regionFrequency,  paletteRow
    /// [4] seed low 16 bits, seed high 16 bits, absorption,       pixelScale
    /// </code>
    /// </remarks>
    public static class NebulaLayerData
    {
        /// <summary>Must match NEBULA_MAX_LAYERS in the shader.</summary>
        public const int MaxLayers = 4;

        /// <summary>Must match NEBULA_VECTORS_PER_LAYER in the shader.</summary>
        public const int VectorsPerLayer = 5;

        /// <summary>
        /// Length of the uniform array. Unity fixes an array's size the
        /// first time it is set, so it is always uploaded at full length.
        /// </summary>
        public const int ArrayLength = MaxLayers * VectorsPerLayer;

        /// <summary>
        /// Writes a layer into its slots of the uniform array.
        /// </summary>
        /// <param name="paletteRow">Row of the layer's palette in the atlas.</param>
        public static void Write(
            NebulaLayer layer,
            NebulaConfig config,
            int paletteRow,
            Vector4[] target,
            int layerIndex)
        {
            int offset = layerIndex * VectorsPerLayer;
            int colors = layer.palette != null ? layer.palette.ToneCount : 0;
            bool enabled = layer.enabled && colors > 0;

            target[offset] = new Vector4(
                enabled ? 1f : 0f,
                (float)layer.kind,
                layer.parallaxFactor,
                layer.intensity
            );

            target[offset + 1] = new Vector4(
                1f / Mathf.Max(0.1f, layer.cloudSize),
                layer.octaves,
                layer.warp,
                layer.threshold
            );

            // One extra tone for empty space.
            target[offset + 2] = new Vector4(
                layer.contrast,
                layer.veil,
                layer.flowRadius,
                colors + 1
            );

            target[offset + 3] = new Vector4(
                layer.regionMin,
                layer.regionMax,
                1f / Mathf.Max(1f, config.regionSize),
                paletteRow
            );

            // A float holds integers exactly only up to 2^24, so the 32-bit
            // seed is split into two 16-bit halves and rebuilt in the shader.
            uint seed = unchecked((uint)config.worldSeed ^ (uint)layer.seedOffset);

            target[offset + 4] = new Vector4(
                seed & 0xFFFFu,
                seed >> 16,
                layer.absorption,
                Mathf.Max(1, layer.pixelScale)
            );
        }
    }
}
