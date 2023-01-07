Shader "Joyrev/FlatToonStep" {
    Properties {
        [MainTexture] _BaseMap("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}
        [MainColor]   _BaseColor("Base Color", Color) = (1, 1, 1, 1)

        [Toggle(_ALPHATEST_ON)] _AlphaTestToggle ("Alpha Clipping", Float) = 0
        _Cutoff ("Alpha Cutoff", Float) = 0.5

        _ShadowSteps ("Shadow Steps", Range(1, 10)) = 4
        _ShadowOffset ("Shadow Offset", Range(0, 5)) = 0.5
        _ShadowDark ("Shadow Darkness", Range(0, 1)) = 0.25
        _ShadowLight ("Shadow Brightness", Range(0, 1)) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True"
        }

        // Global / All Pass Includes
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            int    _ShadowSteps;
            float  _ShadowOffset;
            float  _ShadowDark;
            float  _ShadowLight;

            // for shadow casting
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float _Cutoff;
        CBUFFER_END
        ENDHLSL

        Pass {
            name "ForwardLit"
            Tags {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 position : POSITION;
                float3 normal   : NORMAL;
                float2 uv       : TEXCOORD0;
            };

            struct Varyings {
                float4 position : SV_POSITION;
                float3 normal   : NORMAL;
                float2 uv       : TEXCOORD0;
            };

            #pragma vertex Vert
            #pragma fragment Frag

            Varyings Vert(Attributes i) {
                Varyings o;
                o.position = TransformObjectToHClip(i.position.xyz);
                o.uv = i.uv;
                o.normal = i.normal;
                return o;
            }

            float4 Frag(Varyings i) : SV_Target {
                // -1 to 1, multiplying by 0.5 you get -0.5 to 0.5, adding 0.5, you get 0 to 1
                float3 normal = i.normal * 0.5 + 0.5f;

                // access some lighting info
                Light mainLight = GetMainLight();
                half3 lightDir  = mainLight.direction;
                half3 lightCol  = mainLight.color;

                float lightFalloff = max(0, dot(normal, lightDir));

                // step the shadows
                lightFalloff = lerp(_ShadowDark, _ShadowLight, ceil(lightFalloff * _ShadowSteps + _ShadowOffset) / _ShadowSteps);
                float3 directLight = lightCol * lightFalloff;

                // create some basic ambient light. TODO: Use a property
                float3 ambientLight = float3(0.22, 0.22, 0.22);

                float3 diffuseLight = ambientLight + directLight;
                float3 finalSurfColor = diffuseLight * _BaseColor.rgb;

                return float4(finalSurfColor, 0);
            }

            ENDHLSL
        }

        // ShadowCaster, for casting shadows
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // GPU Instancing
            #pragma multi_compile_instancing
            //#pragma multi_compile _ DOTS_INSTANCING_ON

            // Universal Pipeline Keywords
            // (v11+) This is used during shadow map generation to differentiate between directional and punctual (point/spot) light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        // DepthOnly, used for Camera Depth Texture (if cannot copy depth buffer instead, and the DepthNormals below isn't used)
        Pass {
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }

            ColorMask 0
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // GPU Instancing
            #pragma multi_compile_instancing
            //#pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

        // DepthNormals, used for SSAO & other custom renderer features that request it
        Pass {
            Name "DepthNormals"
            Tags { "LightMode"="DepthNormals" }

            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // GPU Instancing
            #pragma multi_compile_instancing
            //#pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
            ENDHLSL
        }

    }
}
