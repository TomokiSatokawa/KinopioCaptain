
//AI
Shader "Custom/StencilSilhouette/HiddenSilhouette"
{
    Properties
    {
        _SilhouetteColor ("Silhouette Color", Color) = (1, 0, 0, 1)
        _SilhouetteIntensity ("Color Intensity", Range(0, 5)) = 1
        _SilhouetteAlpha ("Alpha", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Geometry+1"
        }

        Pass
        {
            Name "HiddenSilhouette"
            Tags { "LightMode" = "UniversalForwardOnly" }

            Cull Back
            ZTest Greater
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 1
                Comp Equal
                Pass Keep
            }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _SilhouetteColor;
                float _SilhouetteIntensity;
                float _SilhouetteAlpha;
            CBUFFER_END

            struct Attributes
            {
                float3 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 color = _SilhouetteColor;

                color.rgb *= _SilhouetteIntensity;
                color.a *= _SilhouetteAlpha;

                return color;
            }

            ENDHLSL
        }
    }
}
