#ifndef STARFIELD_COMMON_INCLUDED
#define STARFIELD_COMMON_INCLUDED

// Shared by every background layer: camera, pixel grid, hashing, noise
// and the palette atlas. Everything is a pure function of world position,
// seed and time, so revisiting a place reproduces it.

TEXTURE2D(_GradientAtlas);
SAMPLER(sampler_GradientAtlas);
float4 _GradientAtlas_TexelSize;

float4 _CameraPosition;
float _PixelsPerUnit;
float _StarfieldTime;
float _StarfieldExposure;

// ---------------------------------------------------------------------
// Hashing
// ---------------------------------------------------------------------

uint StarfieldHash(uint value)
{
    value ^= value >> 16;
    value *= 0x7feb352du;
    value ^= value >> 15;
    value *= 0x846ca68bu;
    value ^= value >> 16;
    return value;
}

float Random01(uint value)
{
    return (StarfieldHash(value) >> 8) * (1.0 / 16777216.0);
}

// Integer hashing instead of frac(sin()): sin loses precision on WebGL
// and mobile GPUs, which shows up as banding and repeating patterns.
uint HashCell(int2 cell, uint seed)
{
    return StarfieldHash(
        asuint(cell.x) * 0x8da6b343u ^
        asuint(cell.y) * 0xd8163841u ^
        seed
    );
}

// ---------------------------------------------------------------------
// Pixel grid
// ---------------------------------------------------------------------

bool IsPixelSnapped()
{
    return _PixelsPerUnit > 0.0;
}

// Grid density for a layer drawn with blocks of pixelScale x pixelScale
// sprite pixels. 1 is the sprites' own grid.
float GridPixelsPerUnit(float pixelScale)
{
    return _PixelsPerUnit / max(1.0, pixelScale);
}

// Snaps a world-space offset to the centre of its grid cell.
float2 SnapToGrid(float2 position, float gridPixelsPerUnit)
{
    if (!IsPixelSnapped())
        return position;

    return (floor(position * gridPixelsPerUnit) + 0.5) / gridPixelsPerUnit;
}

// Moves a layer in whole grid cells, so it stays on its grid while it
// scrolls instead of sliding through its own blocks.
float2 SnapOffsetToGrid(float2 offset, float gridPixelsPerUnit)
{
    if (!IsPixelSnapped())
        return offset;

    return round(offset * gridPixelsPerUnit) / gridPixelsPerUnit;
}

float2 SnapToPixel(float2 position)
{
    return SnapToGrid(position, _PixelsPerUnit);
}

// Position of a background layer for the current fragment. screenOffset
// is the fragment's world position relative to the camera; pixelScale is
// the size of the layer's blocks in sprite pixels.
float2 LayerPosition(float2 screenOffset, float parallax, float pixelScale)
{
    float grid = GridPixelsPerUnit(pixelScale);

    return SnapToGrid(screenOffset, grid) +
        SnapOffsetToGrid(_CameraPosition.xy * parallax, grid);
}

float2 LayerPosition(float2 screenOffset, float parallax)
{
    return LayerPosition(screenOffset, parallax, 1.0);
}

// 4x4 ordered dithering threshold in [0, 1). Indexed by the pixel cell in
// the layer's own space, so the pattern travels with the layer instead of
// crawling over it.
static const float BayerMatrix[16] =
{
     0.0,  8.0,  2.0, 10.0,
    12.0,  4.0, 14.0,  6.0,
     3.0, 11.0,  1.0,  9.0,
    15.0,  7.0, 13.0,  5.0
};

float BayerThreshold(int2 cell)
{
    int index = (cell.y & 3) * 4 + (cell.x & 3);
    return (BayerMatrix[index] + 0.5) / 16.0;
}

// ---------------------------------------------------------------------
// Gradient noise
// ---------------------------------------------------------------------

static const float2 NoiseGradients[8] =
{
    float2( 1.0,  0.0), float2(-1.0,  0.0),
    float2( 0.0,  1.0), float2( 0.0, -1.0),
    float2( 0.7071,  0.7071), float2(-0.7071,  0.7071),
    float2( 0.7071, -0.7071), float2(-0.7071, -0.7071)
};

float GradientDot(int2 cell, uint seed, float2 offset)
{
    return dot(NoiseGradients[HashCell(cell, seed) & 7u], offset);
}

// Perlin-style gradient noise remapped to roughly [0, 1].
float GradientNoise(float2 position, uint seed)
{
    float2 floored = floor(position);
    float2 f = position - floored;
    int2 cell = (int2)floored;

    float a = GradientDot(cell, seed, f);
    float b = GradientDot(cell + int2(1, 0), seed, f - float2(1.0, 0.0));
    float c = GradientDot(cell + int2(0, 1), seed, f - float2(0.0, 1.0));
    float d = GradientDot(cell + int2(1, 1), seed, f - float2(1.0, 1.0));

    float2 u = f * f * f * (f * (f * 6.0 - 15.0) + 10.0);

    return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y) * 0.7 + 0.5;
}

#define STARFIELD_MAX_OCTAVES 6

// Fractal noise. octaves is clamped to STARFIELD_MAX_OCTAVES; the loop
// bound is a constant so it compiles on WebGL 2.
float Fbm(float2 position, uint seed, int octaves)
{
    float value = 0.0;
    float amplitude = 0.5;
    float total = 0.0;

    [loop]
    for (int i = 0; i < STARFIELD_MAX_OCTAVES; ++i)
    {
        if (i >= octaves)
            break;

        value += amplitude * GradientNoise(position, seed + (uint)i * 101u);
        total += amplitude;
        amplitude *= 0.5;
        position *= 2.03;
    }

    return value / max(total, 0.0001);
}

// ---------------------------------------------------------------------
// Palette atlas: one row per star layer, then one row per nebula layer.
// ---------------------------------------------------------------------

float3 SampleAtlas(float u, uint row)
{
    float halfTexel = 0.5 * _GradientAtlas_TexelSize.x;

    float2 uv = float2(
        lerp(halfTexel, 1.0 - halfTexel, saturate(u)),
        (float(row) + 0.5) * _GradientAtlas_TexelSize.y
    );

    return SAMPLE_TEXTURE2D_LOD(_GradientAtlas, sampler_GradientAtlas, uv, 0).rgb;
}

#endif
