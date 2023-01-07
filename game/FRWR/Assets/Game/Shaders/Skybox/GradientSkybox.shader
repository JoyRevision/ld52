Shader "Joyrev/GradientSkybox" {
    Properties {
        _TopColor ("Top Color", Color) = (1, 0.3, 0.3, 1)
        _MiddleColor ("MiddleColor", Color) = (1.0, 1.0, 1)
        _BottomColor ("Bottom Color", Color) = (0.3, 0.3, 1, 1)

        _Exp ("Exp", Range(0, 16)) = 1
    }
    SubShader {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Background"
            "Queue" = "Background"
            "PreviewType" = "Skybox"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float4 _TopColor;
        float4 _MiddleColor;
        float4 _BottomColor;
        float _Exp;
        CBUFFER_END
        ENDHLSL

        Pass {
            Name "Unlit"

            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            struct Attributes {
                float4 positionOS	: POSITION;
                float3 uv		    : TEXCOORD0;
                float4 color		: COLOR;
            };

            struct Varyings {
                float4 positionCS 	: SV_POSITION;
                float3 uv		    : TEXCOORD0;
                float4 color		: COLOR;
            };

            // Vertex Shader
            Varyings Vertex(Attributes IN) {
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz); // world space
                float4 positionCS = TransformWorldToHClip(positionWS);         // clip space

                Varyings OUT;
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                OUT.positionCS = positionCS;
                return OUT;
            }

            // Fragment Shader
            half4 Fragment(Varyings IN) : SV_Target {
                float3 uv = normalize(IN.uv);
                float3 up = normalize(float3(0,1,0));
                float d = dot(uv, up);
                float s = sign(d);

                return half4(lerp(_MiddleColor, s < 0.0 ? _BottomColor : _TopColor, pow(abs(d), _Exp)).rgb, 1);
            }
            ENDHLSL
        }
    }
}