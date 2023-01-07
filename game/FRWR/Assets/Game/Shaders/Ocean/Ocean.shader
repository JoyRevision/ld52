Shader "Joyrev/Ocean" {
    Properties {
        _OceanBaseColor ("Base Ocean Color", Color) = (0.41, 0.67, 0.66, 1.0)
        _FoamSize ("Foam Size", Range(0, 5)) = 1
    }
    SubShader {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float4 _OceanBaseColor;
        float _FoamSize;
        CBUFFER_END
        ENDHLSL

        Pass {
            Name "Unlit"

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            struct Attributes {
                float4 positionOS	: POSITION;
                float2 uv		    : TEXCOORD0;
                float4 color		: COLOR;
            };

            struct Varyings {
                float4 positionCS 	: SV_POSITION;
                float2 uv		    : TEXCOORD0;
                float4 color		: COLOR;
                float3 positionVS   : TEXCOORD1;
                float4 screenPos    : TEXCOORD2;
            };

            // Vertex Shader
            Varyings Vertex(Attributes IN) {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz); // world space
                float3 positionVS = TransformWorldToView(positionWS);          // view space
                float4 positionCS = TransformWorldToHClip(positionWS);         // clip space

                OUT.uv = IN.uv;
                OUT.color = IN.color;

                OUT.positionVS = positionVS;
                OUT.positionCS = positionCS;
                OUT.screenPos = ComputeScreenPos(positionCS);

                return OUT;
            }

            // Fragment Shader
            half4 Fragment(Varyings IN) : SV_Target {
                float fragmentEyeDepth = -IN.positionVS.z;
                float rawDepth = SampleSceneDepth(IN.screenPos.xy / IN.screenPos.w);
                float sceneEyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                float depthDifference = 1 - saturate((sceneEyeDepth - fragmentEyeDepth) * _FoamSize);

                return _OceanBaseColor + depthDifference;
            }
            ENDHLSL
        }
    }
}