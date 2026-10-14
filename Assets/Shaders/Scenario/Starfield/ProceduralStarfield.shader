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
            #include "StarfieldCommon.hlsl"
            #include "StarfieldStars.hlsl"
            #include "StarfieldNebula.hlsl"

            // Must match SpaceBackgroundRenderer.MaxComposedLayers in C#.
            #define BACKGROUND_MAX_LAYERS 12
            #define BACKGROUND_KIND_STARS 0

            // Star and nebula layers merged and sorted by parallax, from
            // the farthest to the nearest: x = kind (0 stars, 1 nebula),
            // y = index in that kind's array.
            float4 _LayerOrder[BACKGROUND_MAX_LAYERS];
            int _LayerOrderCount;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;

                // Fragment position in world units relative to the
                // camera, independent of how far the quad overscans.
                float2 screenOffset : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);

                output.positionCS = TransformWorldToHClip(positionWS);
                output.screenOffset = positionWS.xy - _CameraPosition.xy;

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float3 color = 0.0;

                // Before the loop: derivatives are undefined once it has
                // branched per pixel.
                float2 footprint =
                    abs(ddx(input.screenOffset)) + abs(ddy(input.screenOffset));

                float screenPixelSize = max(footprint.x, footprint.y);

                NebulaInfluence influence;
                influence.Density = 0.0;
                influence.Region = 0.0;
                influence.Tint = 0.0;

                int count = min(_LayerOrderCount, BACKGROUND_MAX_LAYERS);

                // Back to front: each layer covers or tints what is
                // already behind it, the way depth would.
                [loop]
                for (int i = 0; i < count; ++i)
                {
                    float4 entry = _LayerOrder[i];
                    uint index = (uint)(entry.y + 0.5);

                    if ((int)(entry.x + 0.5) == BACKGROUND_KIND_STARS)
                    {
                        color += GenerateStarLayer(
                            input.screenOffset,
                            screenPixelSize,
                            index,
                            influence
                        );

                        continue;
                    }

                    NebulaLayerData layer = LoadNebulaLayer(index);
                    NebulaSample nebula = SampleNebula(input.screenOffset, index);

                    if (layer.Kind == NEBULA_KIND_EMISSION)
                    {
                        // Stars in front of a cloud cluster and glow
                        // even where the dithering leaves a gap.
                        influence.Region = max(influence.Region, nebula.Region);

                        if (nebula.Density <= 0.0)
                            continue;

                        // A veil dims what is behind before the gas adds
                        // its own light, so the cloud has body.
                        color *= 1.0 - layer.Veil * nebula.Density;
                        color += nebula.Color * layer.Intensity;

                        influence.Density = max(influence.Density, nebula.Density);
                        influence.Tint = nebula.Color * layer.Intensity;
                    }
                    else if (nebula.Density > 0.0)
                    {
                        // Dust blocks everything behind it and leaves the
                        // layers in front untouched.
                        color = lerp(
                            color,
                            nebula.Color * layer.Intensity,
                            saturate(layer.Absorption * nebula.Density)
                        );
                    }
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
