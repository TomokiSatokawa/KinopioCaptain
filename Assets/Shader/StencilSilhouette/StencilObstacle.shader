//AI
Shader "Custom/StencilSilhouette/Obstacle"
{
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "StencilOnly"
            Tags { "LightMode" = "UniversalForwardOnly" }

            Cull Back
            ZTest Equal
            ZWrite Off
            ColorMask 0

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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
                // ColorMask 0 prevents this pass from changing the color buffer.
                return half4(0, 0, 0, 0);
            }

            ENDHLSL
        }
    }

    Fallback Off
}
