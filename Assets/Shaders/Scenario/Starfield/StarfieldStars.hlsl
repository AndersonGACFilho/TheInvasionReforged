#ifndef STARFIELD_STARS_INCLUDED
#define STARFIELD_STARS_INCLUDED

#include "StarfieldCommon.hlsl"

// Must match StarfieldLayerData.MaxLayers and
// StarfieldLayerData.VectorsPerLayer in C#.
#define STARFIELD_MAX_LAYERS 8
#define STARFIELD_VECTORS_PER_LAYER 5

struct StarfieldLayerData
{
    bool Enabled;
    float ParallaxFactor;
    float Density;
    float MinSize;

    float MaxSize;
    float CellSize;
    float Brightness;
    float TwinkleIntensity;

    float MinTwinkleFrequency;
    float MaxTwinkleFrequency;
    float TwinkleProbability;
    float GlowIntensity;

    float GlowProbability;
    float FlareProbability;
    float FlareIntensity;
    float FlareLength;

    // World seed combined with the layer's seed offset.
    uint Seed;
};

// Layer settings as a uniform array rather than a StructuredBuffer,
// which WebGL 2 does not have. The layout is documented in
// StarfieldLayerData.cs.
float4 _StarLayerParams[STARFIELD_MAX_LAYERS * STARFIELD_VECTORS_PER_LAYER];

// How star layers react to the emission nebulae behind them.
float _NebulaStarHalo;
float _NebulaStarCluster;

// What the emission nebulae composed so far left at this fragment.
struct NebulaInfluence
{
    // Quantized cloud density, drives the halo.
    float Density;

    // Smooth region weight, drives clustering. Smooth on purpose: every
    // pixel of a star must agree on whether the star exists.
    float Region;

    float3 Tint;
};

StarfieldLayerData LoadStarLayer(uint layerIndex)
{
    uint offset = layerIndex * STARFIELD_VECTORS_PER_LAYER;

    float4 v0 = _StarLayerParams[offset];
    float4 v1 = _StarLayerParams[offset + 1];
    float4 v2 = _StarLayerParams[offset + 2];
    float4 v3 = _StarLayerParams[offset + 3];
    float4 v4 = _StarLayerParams[offset + 4];

    StarfieldLayerData layer;

    layer.Enabled = v0.x > 0.5;
    layer.ParallaxFactor = v0.y;
    layer.Density = v0.z;
    layer.MinSize = v0.w;

    layer.MaxSize = v1.x;
    layer.CellSize = v1.y;
    layer.Brightness = v1.z;
    layer.TwinkleIntensity = v1.w;

    layer.MinTwinkleFrequency = v2.x;
    layer.MaxTwinkleFrequency = v2.y;
    layer.TwinkleProbability = v2.z;
    layer.GlowIntensity = v2.w;

    layer.GlowProbability = v3.x;
    layer.FlareProbability = v3.y;
    layer.FlareIntensity = v3.z;
    layer.FlareLength = v3.w;

    // The seed arrives as two exact 16-bit halves.
    layer.Seed = (uint)v4.x | ((uint)v4.y << 16);

    return layer;
}

uint CellSeed(int2 cell, uint layerIndex, uint layerSeed)
{
    uint seed = layerSeed;

    seed ^= StarfieldHash(asuint(cell.x) + 0x9e3779b9u);
    seed ^= StarfieldHash(asuint(cell.y) + 0x85ebca6bu);
    seed ^= StarfieldHash(layerIndex + 0xc2b2ae35u);

    return StarfieldHash(seed);
}

float CalculateGlow(float distanceToStar, float radius, float intensity)
{
    float haloScale = max(radius * 3.5, 0.0001);
    float normalizedDistance = distanceToStar / haloScale;

    return exp(-4.0 * normalizedDistance * normalizedDistance) *
        max(0.0, intensity);
}

float CalculateFlare(
    float2 delta,
    float radius,
    float intensity,
    float flareLength)
{
    float lengthScale = max(radius * max(flareLength, 1.0), 0.0001);
    float rayWidth = max(radius * 0.15, 0.0001);

    float horizontal =
        exp(-abs(delta.y) / rayWidth) *
        exp(-abs(delta.x) / lengthScale);

    float vertical =
        exp(-abs(delta.x) / rayWidth) *
        exp(-abs(delta.y) / lengthScale);

    return (horizontal + vertical) * max(0.0, intensity);
}

float CalculateTwinkle(
    StarfieldLayerData layer,
    uint seed,
    out float amplitude)
{
    amplitude = 0.0;

    if (Random01(seed ^ 0x61aabbccu) >= saturate(layer.TwinkleProbability))
        return 1.0;

    float minFrequency = max(0.0, layer.MinTwinkleFrequency);
    float maxFrequency = max(minFrequency, layer.MaxTwinkleFrequency);

    float frequency = lerp(
        minFrequency,
        maxFrequency,
        Random01(seed ^ 0x71aabbccu)
    );

    float phase = Random01(seed ^ 0x81aabbccu) * TWO_PI;

    amplitude =
        saturate(layer.TwinkleIntensity) *
        lerp(0.5, 1.0, Random01(seed ^ 0x91aabbccu));

    float oscillation = sin(TWO_PI * frequency * _StarfieldTime + phase);

    return 1.0 + amplitude * oscillation;
}

float CalculateFlareActivation(float twinkle, float amplitude)
{
    if (amplitude <= 0.0001)
        return 0.0;

    float normalizedPeak = saturate((twinkle - 1.0) / amplitude);

    return smoothstep(0.3, 0.85, normalizedPeak);
}

// screenPixelSize is one screen pixel in world units. It is computed by
// the caller before the layer loop: derivatives are undefined once the
// loop has branched per pixel.
float3 GenerateStarLayer(
    float2 screenOffset,
    float screenPixelSize,
    uint layerIndex,
    NebulaInfluence nebula)
{
    StarfieldLayerData layer = LoadStarLayer(layerIndex);

    if (!layer.Enabled ||
        layer.Density <= 0.0 ||
        layer.Brightness <= 0.0)
    {
        return 0.0;
    }

    float cellSize = max(layer.CellSize, 0.001);

    float2 samplePosition =
        LayerPosition(screenOffset, layer.ParallaxFactor);

    // On the sprite grid a star is made of whole sprite pixels, so its
    // edge is hard; otherwise it is antialiased over one screen pixel.
    float pixelSize = IsPixelSnapped()
        ? 1.0 / _PixelsPerUnit
        : screenPixelSize;

    float aa = IsPixelSnapped() ? 0.0001 : max(pixelSize * 0.5, 0.0001);

    // Stars gather where emission nebulae are, like star-forming regions.
    float density = saturate(
        layer.Density * (1.0 + _NebulaStarCluster * nebula.Region)
    );

    int2 centerCell = (int2)floor(samplePosition / cellSize);

    float3 result = 0.0;

    // Evaluate neighboring cells to avoid clipping stars near cell
    // boundaries.
    [unroll]
    for (int y = -1; y <= 1; ++y)
    {
        [unroll]
        for (int x = -1; x <= 1; ++x)
        {
            int2 cell = centerCell + int2(x, y);

            uint seed = CellSeed(cell, layerIndex, layer.Seed);

            if (Random01(seed ^ 0x11aabbccu) >= density)
                continue;

            float2 jitter = float2(
                Random01(seed ^ 0x21aabbccu),
                Random01(seed ^ 0x31aabbccu)
            );

            float2 starPosition = (float2(cell) + jitter) * cellSize;

            if (IsPixelSnapped())
                starPosition = SnapToPixel(starPosition);

            float radius = lerp(
                max(0.0001, layer.MinSize),
                max(layer.MinSize, layer.MaxSize),
                Random01(seed ^ 0x41aabbccu)
            );

            radius = max(radius, pixelSize * 0.5);

            float2 delta = samplePosition - starPosition;
            float distanceToStar = length(delta);

            float core = 1.0 - smoothstep(
                radius - aa,
                radius + aa,
                distanceToStar
            );

            // Glow: selected stars only.
            float glow = 0.0;

            if (Random01(seed ^ 0xB17E53A9u) < saturate(layer.GlowProbability))
            {
                glow = CalculateGlow(
                    distanceToStar,
                    radius,
                    layer.GlowIntensity
                );
            }

            // Flare: selected stars only.
            float flare = 0.0;

            if (Random01(seed ^ 0xA13F28B5u) < saturate(layer.FlareProbability))
            {
                flare = CalculateFlare(
                    delta,
                    radius,
                    layer.FlareIntensity,
                    layer.FlareLength
                );
            }

            float amplitude;
            float twinkle = CalculateTwinkle(layer, seed, amplitude);

            // Flare appears near brightness peaks; glow follows the
            // star's brightness.
            flare *= CalculateFlareActivation(twinkle, amplitude);
            float animatedGlow = glow * max(0.0, twinkle);

            float contribution = core * twinkle + animatedGlow + flare;

            // A star inside an emission cloud lights it up around itself.
            float nebulaHalo = CalculateGlow(
                distanceToStar,
                radius * 1.5,
                _NebulaStarHalo * nebula.Density
            ) * max(0.0, twinkle);

            if (contribution < 0.0001 && nebulaHalo < 0.0001)
                continue;

            float colorRandom = Random01(seed ^ 0x51aabbccu);
            float3 starColor = SampleAtlas(colorRandom, layerIndex);

            result +=
                starColor * layer.Brightness * contribution +
                nebula.Tint * nebulaHalo;
        }
    }

    return result;
}

#endif
