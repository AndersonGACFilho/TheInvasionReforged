#ifndef STARFIELD_NEBULA_INCLUDED
#define STARFIELD_NEBULA_INCLUDED

#include "StarfieldCommon.hlsl"

// Must match NebulaLayerData.MaxLayers and NebulaLayerData.VectorsPerLayer
// in C#.
#define NEBULA_MAX_LAYERS 4
#define NEBULA_VECTORS_PER_LAYER 5

#define NEBULA_KIND_EMISSION 0
#define NEBULA_KIND_DUST 1

struct NebulaLayerData
{
    bool Enabled;
    int Kind;
    float ParallaxFactor;
    float Intensity;

    float Frequency;
    int Octaves;
    float Warp;
    float Threshold;

    float Contrast;
    float Veil;
    float FlowRadius;

    // Palette colors plus one for empty space.
    int ToneCount;

    float RegionMin;
    float RegionMax;
    float RegionFrequency;
    uint PaletteRow;

    // World seed combined with the layer's seed offset.
    uint Seed;
    float Absorption;

    // Size of the layer's blocks in sprite pixels.
    float PixelScale;
};

// Layout documented in NebulaLayerData.cs.
float4 _NebulaLayerParams[NEBULA_MAX_LAYERS * NEBULA_VECTORS_PER_LAYER];

// Position of each layer in its flow cycle, in [0, 1). Computed on the
// CPU from the nebula clock so the shader never sees a large time value.
float _NebulaPhase[NEBULA_MAX_LAYERS];

struct NebulaSample
{
    // Quantized density in [0, 1]; 0 means no cloud at this pixel.
    float Density;

    // Smooth region weight in [0, 1].
    float Region;

    float3 Color;
};

NebulaLayerData LoadNebulaLayer(uint layerIndex)
{
    uint offset = layerIndex * NEBULA_VECTORS_PER_LAYER;

    float4 v0 = _NebulaLayerParams[offset];
    float4 v1 = _NebulaLayerParams[offset + 1];
    float4 v2 = _NebulaLayerParams[offset + 2];
    float4 v3 = _NebulaLayerParams[offset + 3];
    float4 v4 = _NebulaLayerParams[offset + 4];

    NebulaLayerData layer;

    layer.Enabled = v0.x > 0.5;
    layer.Kind = (int)(v0.y + 0.5);
    layer.ParallaxFactor = v0.z;
    layer.Intensity = v0.w;

    layer.Frequency = v1.x;
    layer.Octaves = (int)(v1.y + 0.5);
    layer.Warp = v1.z;
    layer.Threshold = v1.w;

    layer.Contrast = v2.x;
    layer.Veil = v2.y;
    layer.FlowRadius = v2.z;
    layer.ToneCount = (int)(v2.w + 0.5);

    layer.RegionMin = v3.x;
    layer.RegionMax = v3.y;
    layer.RegionFrequency = v3.z;
    layer.PaletteRow = (uint)(v3.w + 0.5);

    // The seed arrives as two exact 16-bit halves.
    layer.Seed = (uint)v4.x | ((uint)v4.y << 16);
    layer.Absorption = v4.z;
    layer.PixelScale = max(1.0, v4.w);

    return layer;
}

// Where in the world this layer is allowed to appear. A very low
// frequency noise splits space into regions, so each kind of nebula owns
// parts of the map instead of covering everything.
float NebulaRegionWeight(NebulaLayerData layer, float2 position)
{
    if (layer.RegionMin <= 0.0 && layer.RegionMax >= 1.0)
        return 1.0;

    float mask = GradientNoise(
        position * layer.RegionFrequency,
        layer.Seed ^ 0x5bd1e995u
    );

    const float edge = 0.06;

    return smoothstep(layer.RegionMin - edge, layer.RegionMin + edge, mask) *
        (1.0 - smoothstep(layer.RegionMax - edge, layer.RegionMax + edge, mask));
}

NebulaSample SampleNebula(float2 screenOffset, uint layerIndex)
{
    NebulaSample result;
    result.Density = 0.0;
    result.Region = 0.0;
    result.Color = 0.0;

    NebulaLayerData layer = LoadNebulaLayer(layerIndex);

    if (!layer.Enabled || layer.Intensity <= 0.0 || layer.ToneCount < 2)
        return result;

    float2 position = LayerPosition(
        screenOffset,
        layer.ParallaxFactor,
        layer.PixelScale
    );

    result.Region = NebulaRegionWeight(layer, position);

    if (result.Region <= 0.0)
        return result;

    float2 p = position * layer.Frequency;

    // The large shape depends only on position, so it stays put and can
    // be recognized when revisited. Time only moves the warp around a
    // circle in noise space: details flow like slow gas and return to
    // the exact same shape after each cycle.
    float angle = TWO_PI * _NebulaPhase[layerIndex];
    float2 orbit = layer.FlowRadius * float2(cos(angle), sin(angle));

    float2 warp = float2(
        Fbm(p + orbit + float2(3.1, 1.7), layer.Seed + 11u, 3),
        Fbm(p - orbit + float2(-2.3, 5.2), layer.Seed + 23u, 3)
    );

    float n = Fbm(
        p + layer.Warp * (warp - 0.5) * 2.0,
        layer.Seed,
        layer.Octaves
    );

    n = saturate((n - layer.Threshold) * layer.Contrast) * result.Region;

    // A few discrete tones with ordered dithering between them, like a
    // hand-made pixel art palette.
    // Dither per block, so a coarse layer dithers in coarse blocks.
    int2 pixelCell = IsPixelSnapped()
        ? (int2)floor(position * GridPixelsPerUnit(layer.PixelScale))
        : (int2)floor(position * 64.0);

    float steps = (float)(layer.ToneCount - 1);
    float tone = clamp(floor(n * steps + BayerThreshold(pixelCell)), 0.0, steps);

    if (tone <= 0.0)
        return result;

    result.Density = tone / steps;

    // Tone 0 is empty space; tones 1..N use the N palette colors, each
    // sampled at the centre of its segment of the atlas row.
    result.Color = SampleAtlas((tone - 0.5) / steps, layer.PaletteRow);

    return result;
}

#endif
