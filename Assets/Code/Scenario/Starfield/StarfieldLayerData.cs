using UnityEngine;

/// <summary>
///     Packs starfield layers into the float4 uniform array read by
///     the ProceduralStarfield shader.
/// </summary>
/// <remarks>
///     <para>
///         Layers travel as a uniform array instead of a StructuredBuffer
///         because WebGL 2 has no structured buffers. Every field goes to an
///         explicit slot, so the C# and HLSL sides cannot drift apart by
///         field order.
///     </para>
///     <para>
///         Layout per layer, matching LoadLayer in the shader:
///         <code>
/// [0] enabled,               parallaxFactor,        density,            minSize
/// [1] maxSize,               cellSize,              brightness,         twinkleIntensity
/// [2] minTwinkleFrequency,   maxTwinkleFrequency,   twinkleProbability, glowIntensity
/// [3] glowProbability,       flareProbability,      flareIntensity,     flareLength
/// [4] seed low 16 bits,      seed high 16 bits,     unused,             unused
/// </code>
///     </para>
/// </remarks>
public static class StarfieldLayerData
{
    /// <summary>
    ///     Maximum number of layers the shader evaluates.
    ///     Must match STARFIELD_MAX_LAYERS in the shader.
    /// </summary>
    public const int MaxLayers = 8;

    /// <summary>
    ///     Number of float4 values used by each layer.
    ///     Must match STARFIELD_VECTORS_PER_LAYER in the shader.
    /// </summary>
    public const int VectorsPerLayer = 5;

    /// <summary>
    ///     Length of the uniform array. Unity fixes an array's size the
    ///     first time it is set, so it is always uploaded at full length.
    /// </summary>
    public const int ArrayLength = MaxLayers * VectorsPerLayer;

    /// <summary>
    ///     Writes a layer into its slots of the uniform array.
    /// </summary>
    /// <param name="layer">Layer settings from the configuration.</param>
    /// <param name="worldSeed">Seed shared by every layer.</param>
    /// <param name="target">Uniform array of <see cref="ArrayLength" />.</param>
    /// <param name="layerIndex">Index of the layer in the shader.</param>
    public static void Write(
        StarLayer layer,
        int worldSeed,
        Vector4[] target,
        int layerIndex)
    {
        var offset = layerIndex * VectorsPerLayer;

        target[offset] = new Vector4(
            layer.enabled ? 1f : 0f,
            layer.parallaxFactor,
            layer.density,
            layer.minSize
        );

        target[offset + 1] = new Vector4(
            layer.maxSize,
            layer.cellSize,
            layer.brightness,
            layer.twinkleIntensity
        );

        target[offset + 2] = new Vector4(
            layer.minTwinkleFrequency,
            layer.maxTwinkleFrequency,
            layer.twinkleProbability,
            layer.glowIntensity
        );

        target[offset + 3] = new Vector4(
            layer.glowProbability,
            layer.flareProbability,
            layer.flareIntensity,
            layer.flareLength
        );

        // A float holds integers exactly only up to 2^24, so the 32-bit
        // seed is split into two 16-bit halves and rebuilt in the shader.
        var seed = unchecked((uint)worldSeed ^ (uint)layer.seedOffset);

        target[offset + 4] = new Vector4(
            seed & 0xFFFFu,
            seed >> 16,
            0f,
            0f
        );
    }

    /// <summary>
    ///     Updates only the enabled flag of a layer already written.
    /// </summary>
    /// <returns>True when the flag changed.</returns>
    public static bool WriteEnabled(
        bool enabled,
        Vector4[] target,
        int layerIndex)
    {
        var offset = layerIndex * VectorsPerLayer;
        var value = enabled ? 1f : 0f;

        if (Mathf.Approximately(target[offset].x, value))
            return false;

        target[offset].x = value;
        return true;
    }
}
