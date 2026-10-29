Shader "Custom/URPGemPrismVividTransparent"
{
    Properties
    {
        [Header(Color Settings)]
        _BaseColor ("Base Color", Color) = (0.5, 0.8, 1.0, 1.0)
        _EmissionBoost ("Emission/Brightness Boost", Range(1.0, 5.0)) = 2.0

        [Header(Prism Settings)]
        _IridescenceStrength ("Prism Strength", Range(0.0, 3.0)) = 2.0
        _FresnelPower ("Fresnel Power", Range(0.2, 5.0)) = 2.0

        [Header(Transparency Settings)]
        _Alpha ("Base Alpha", Range(0.0, 1.0)) = 0.6
        _RimAlphaBoost ("Rim Alpha Boost", Range(0.0, 1.0)) = 0.8
    }

    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline" 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
        }

        Pass
        {
            Name "PrismPass"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 normalWS     : TEXCOORD0;
                float3 viewDirWS    : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _EmissionBoost;
                float _IridescenceStrength;
                float _FresnelPower;
                float _Alpha;
                float _RimAlphaBoost;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);

                output.positionCS = vertexInput.positionCS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);

                return output;
            }

            // 鮮やかな虹色生成関数（Cosine Gradient）
            float3 Rainbow(float t)
            {
                float3 a = float3(0.5, 0.5, 0.5);
                float3 b = float3(0.5, 0.5, 0.5);
                float3 c = float3(1.0, 1.0, 1.0);
                float3 d = float3(0.0, 0.33, 0.67);
                return a + b * cos(6.28318 * (c * t + d));
            }

            float4 frag(Varyings input) : SV_Target
            {
                float3 normal = normalize(input.normalWS);
                float3 viewDir = normalize(input.viewDirWS);

                // 1. フレネル反射の計算
                float NdotV = saturate(dot(normal, viewDir));
                float fresnel = pow(1.0 - NdotV, _FresnelPower);

                // 2. 角度に応じた虹色の算出（変化を大きくする）
                float prismCoord = saturate((1.0 - NdotV) * 1.5 + fresnel * 0.5);
                float3 rainbowColor = Rainbow(prismCoord);

                // 3. ベースカラーと虹色を強力に合成（発光感・ビビッドさをアップ）
                float3 baseGlow = _BaseColor.rgb * _EmissionBoost;
                float3 prismGlow = rainbowColor * _IridescenceStrength;
                float3 finalColor = baseGlow + (prismGlow * fresnel);

                // 4. アルファ値（中心は程よく透け、輪郭はくっきり発色）
                float finalAlpha = saturate(_Alpha + (fresnel * _RimAlphaBoost));

                return float4(finalColor, finalAlpha);
            }
            ENDHLSL
        }
    }
}