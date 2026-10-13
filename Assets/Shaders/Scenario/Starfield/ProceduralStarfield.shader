
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
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Cull Off
            ZWrite Off
            ZTest LEqual
            Blend Off

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct StarfieldLayerData
            {
                uint Enabled;
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
                uint SeedOffset;
                float FlareProbability;
                float FlareIntensity;

                float FlareLength;
                float Padding0;
                float Padding1;
                float Padding2;
            };

            StructuredBuffer<StarfieldLayerData> _StarLayers;

            TEXTURE2D(_GradientAtlas);
            SAMPLER(sampler_GradientAtlas);

            int _LayerCount;
            int _WorldSeed;

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
                uint seedOffset)
            {
                uint seed = asuint(_WorldSeed) ^ seedOffset;

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

                uint atlasWidth;
                uint atlasHeight;

                _GradientAtlas.GetDimensions(
                    atlasWidth,
                    atlasHeight
                );

                float halfTexel =
                    0.5 / max(1.0, (float)atlasWidth);

                float2 gradientUV = float2(
                    lerp(
                        halfTexel,
                        1.0 - halfTexel,
                        colorRandom
                    ),
                    (float(layerIndex) + 0.5)
                    / max(1.0, (float)_LayerCount)
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
                StarfieldLayerData layer = _StarLayers[layerIndex];

                if (layer.Enabled == 0u ||
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
                            layer.SeedOffset
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

                [loop]
                for (int layer = 0; layer < _LayerCount; ++layer)
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
