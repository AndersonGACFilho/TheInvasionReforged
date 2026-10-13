Shader "TheInvasion/Starfield/Procedural"
{
    Properties
    {
        _GradientAtlas ("Gradient Atlas", 2D) = "white" {}
        _StarfieldExposure ("Starfield Exposure", Range(-4, 4)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Background"
        }

        Pass
        {
            Name "Starfield"
            Tags
            {
                "LightMode" = "SRPDefaultUnlit"
            }

            Cull Off
            ZWrite Off
            ZTest LEqual
            Blend Off

            HLSLPROGRAM
            // 3.5 is what WebGL 2 supports: integer maths and dynamic
            // loops, but no structured buffers or compute.
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            // Layer settings as a uniform array rather than a
            // StructuredBuffer, which WebGL 2 does not have. The layout
            // is documented in StarfieldLayerData.cs.
            float4 _LayerParams[
                STARFIELD_MAX_LAYERS * STARFIELD_VECTORS_PER_LAYER
            ];

            TEXTURE2D(_GradientAtlas);
            SAMPLER(sampler_GradientAtlas);
            float4 _GradientAtlas_TexelSize;

            int _LayerCount;

            float4 _CameraPosition;
            float4 _CameraWorldSize;
            float _StarfieldTime;
            float _StarfieldExposure;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            StarfieldLayerData LoadLayer(uint layerIndex)
            {
                uint offset =
                    layerIndex * STARFIELD_VECTORS_PER_LAYER;

                float4 v0 = _LayerParams[offset];
                float4 v1 = _LayerParams[offset + 1];
                float4 v2 = _LayerParams[offset + 2];
                float4 v3 = _LayerParams[offset + 3];
                float4 v4 = _LayerParams[offset + 4];

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
                layer.Seed =
                    (uint)v4.x | ((uint)v4.y << 16);

                return layer;
            }

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
                return (StarfieldHash(value) >> 8)
                    * (1.0 / 16777216.0);
            }

            uint CellSeed(
                int2 cell,
                uint layerIndex,
                uint layerSeed)
            {
                uint seed = layerSeed;

                seed ^= StarfieldHash(
                    asuint(cell.x) + 0x9e3779b9u
                );

                seed ^= StarfieldHash(
                    asuint(cell.y) + 0x85ebca6bu
                );

                seed ^= StarfieldHash(
                    layerIndex + 0xc2b2ae35u
                );

                return StarfieldHash(seed);
            }

            float CalculateGlow(
                float distanceToStar,
                float radius,
                float intensity)
            {
                float haloScale = max(radius * 3.5, 0.0001);
                float normalizedDistance =
                    distanceToStar / haloScale;

                return exp(
                    -4.0 *
                    normalizedDistance *
                    normalizedDistance
                ) * max(0.0, intensity);
            }


            float CalculateFlare(
                float2 delta,
                float radius,
                float intensity,
                float flareLength)
            {
                float lengthScale = max(
                    radius * max(flareLength, 1.0),
                    0.0001
                );

                float rayWidth = max(
                    radius * 0.15,
                    0.0001
                );

                float horizontal =
                    exp(-abs(delta.y) / rayWidth) *
                    exp(-abs(delta.x) / lengthScale);

                float vertical =
                    exp(-abs(delta.x) / rayWidth) *
                    exp(-abs(delta.y) / lengthScale);

                return (horizontal + vertical)
                    * max(0.0, intensity);
            }

            float CalculateTwinkle(
                StarfieldLayerData layer,
                uint seed,
                out float amplitude)
            {
                amplitude = 0.0;

                if (Random01(seed ^ 0x61aabbccu)
                    >= saturate(layer.TwinkleProbability))
                {
                    return 1.0;
                }

                float minFrequency = max(
                    0.0,
                    layer.MinTwinkleFrequency
                );

                float maxFrequency = max(
                    minFrequency,
                    layer.MaxTwinkleFrequency
                );

                float frequency = lerp(
                    minFrequency,
                    maxFrequency,
                    Random01(seed ^ 0x71aabbccu)
                );

                float phase =
                    Random01(seed ^ 0x81aabbccu) * TWO_PI;

                amplitude =
                    saturate(layer.TwinkleIntensity) *
                    lerp(
                        0.5,
                        1.0,
                        Random01(seed ^ 0x91aabbccu)
                    );

                float oscillation = sin(
                    TWO_PI * frequency *
                    _StarfieldTime + phase
                );

                return 1.0 + amplitude * oscillation;
            }

            float CalculateFlareActivation(
                float twinkle,
                float amplitude)
            {
                if (amplitude <= 0.0001)
                    return 0.0;

                float normalizedPeak = saturate(
                    (twinkle - 1.0) / amplitude
                );

                return smoothstep(
                    0.3,
                    0.85,
                    normalizedPeak
                );
            }

            float3 SampleStarColor(
                uint seed,
                uint layerIndex)
            {
                float colorRandom =
                    Random01(seed ^ 0x51aabbccu);

                // _TexelSize is (1/width, 1/height, width, height),
                // filled in by Unity. One atlas row per layer.
                float halfTexel =
                    0.5 * _GradientAtlas_TexelSize.x;

                float2 gradientUV = float2(
                    lerp(
                        halfTexel,
                        1.0 - halfTexel,
                        colorRandom
                    ),
                    (float(layerIndex) + 0.5)
                    * _GradientAtlas_TexelSize.y
                );

                return SAMPLE_TEXTURE2D_LOD(
                    _GradientAtlas,
                    sampler_GradientAtlas,
                    gradientUV,
                    0
                ).rgb;
            }

            float3 GenerateLayer(
                float2 screenUV,
                uint layerIndex)
            {
                StarfieldLayerData layer = LoadLayer(layerIndex);

                if (!layer.Enabled ||
                    layer.Density <= 0.0 ||
                    layer.Brightness <= 0.0)
                {
                    return 0.0;
                }

                float cellSize = max(
                    layer.CellSize,
                    0.001
                );

                float2 samplePosition =
                    (screenUV - 0.5) * _CameraWorldSize.xy +
                    _CameraPosition.xy * layer.ParallaxFactor;

                // Derivatives are calculated outside
                // divergent branches.
                float2 pixelFootprint =
                    abs(ddx(samplePosition)) +
                    abs(ddy(samplePosition));

                float pixelSize = max(
                    pixelFootprint.x,
                    pixelFootprint.y
                );

                float aa = max(
                    pixelSize * 0.5,
                    0.0001
                );

                int2 centerCell = (int2)floor(
                    samplePosition / cellSize
                );

                float3 result = 0.0;

                // Evaluate neighboring cells to avoid
                // clipping stars near cell boundaries.
                [unroll]
                for (int y = -1; y <= 1; ++y)
                {
                    [unroll]
                    for (int x = -1; x <= 1; ++x)
                    {
                        int2 cell =
                            centerCell + int2(x, y);

                        uint seed = CellSeed(
                            cell,
                            layerIndex,
                            layer.Seed
                        );

                        if (Random01(seed ^ 0x11aabbccu)
                            >= saturate(layer.Density))
                        {
                            continue;
                        }

                        // Star position
                        float2 jitter = float2(
                            Random01(seed ^ 0x21aabbccu),
                            Random01(seed ^ 0x31aabbccu)
                        );

                        float2 starPosition =
                            (float2(cell) + jitter) *
                            cellSize;

                        // Star size
                        float sizeRandom =
                            Random01(seed ^ 0x41aabbccu);

                        float radius = lerp(
                            max(0.0001, layer.MinSize),
                            max(
                                layer.MinSize,
                                layer.MaxSize
                            ),
                            sizeRandom
                        );

                        radius = max(
                            radius,
                            pixelSize * 0.5
                        );

                        float2 delta =
                            samplePosition - starPosition;

                        float distanceToStar =
                            length(delta);

                        // Core
                        float core = 1.0 - smoothstep(
                            radius - aa,
                            radius + aa,
                            distanceToStar
                        );

                        // Glow: selected stars only
                        float halo = 0.0;

                        if (Random01(seed ^ 0xB17E53A9u)
                            < saturate(layer.GlowProbability))
                        {
                            halo = CalculateGlow(
                                distanceToStar,
                                radius,
                                layer.GlowIntensity
                            );
                        }

                        // Flare: selected stars only
                        float flare = 0.0;

                        if (Random01(seed ^ 0xA13F28B5u)
                            < saturate(layer.FlareProbability))
                        {
                            flare = CalculateFlare(
                                delta,
                                radius,
                                layer.FlareIntensity,
                                layer.FlareLength
                            );
                        }

                        // Independent temporal animation
                        float amplitude;
                        float twinkle = CalculateTwinkle(
                            layer,
                            seed,
                            amplitude
                        );

                        // Flare appears near brightness peaks
                        flare *= CalculateFlareActivation(
                            twinkle,
                            amplitude
                        );

                        // Glow follows the star's brightness
                        float animatedHalo =
                            halo * max(0.0, twinkle);

                        float contribution =
                            core * twinkle +
                            animatedHalo +
                            flare;

                        if (contribution < 0.0001)
                            continue;

                        float3 starColor = SampleStarColor(
                            seed,
                            layerIndex
                        );

                        // Accumulate exactly once
                        result += starColor *
                            layer.Brightness *
                            contribution;
                    }
                }

                return result;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float3 color = 0.0;

                int layerCount = min(
                    _LayerCount,
                    STARFIELD_MAX_LAYERS
                );

                [loop]
                for (int layer = 0; layer < layerCount; ++layer)
                {
                    color += GenerateLayer(
                        input.uv,
                        (uint)layer
                    );
                }

                // Exposure in stops, preserving HDR values.
                color *= exp2(_StarfieldExposure);

                return half4(color, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
