Shader "Custom/Water" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "bump" {}
        _Distortion ("Distortion", Range(0, 1)) = 0.1
        _Speed ("Speed", Range(-1, 1)) = 0.1
        _NoiseScale ("Noise Scale", Range(1, 50)) = 10
    }

    SubShader {
        Tags {"RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100

        Pass {
            Stencil {
                Ref 1
                Comp always
                Pass replace
                Fail keep
                ZFail keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariables.hlsl"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float _Distortion;
            float _Speed;
            float _NoiseScale;

            v2f vert (appdata v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.vertex);
                return o;
            }

            float2 Rand(float2 co){
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            float2 Distort(float2 uv, float time) {
                float2 noise = (tex2D(_BumpMap, uv * _NoiseScale).xy - 0.5) * 2;
                return uv + noise * _Distortion + sin(uv.y * 40 + time * _Speed) * 0.005;
            }

            float4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;

                // Distort the UV
                float time = _Time.y;
                uv = Distort(uv, time);

                // Get the water color
                float4 waterColor = tex2D(_MainTex, uv);

                // Apply reflection and refraction
                float4 refraction = UNITY_SAMPLE_TEX2D_SAMPLER(unity_reflection_refraction, GetRefractionTextureSampler(UNITY_NEAR_CLIP_VALUE), i.worldPos.xy / i.worldPos.w);
                float4 reflection = UNITY_SAMPLE_TEX2D_SAMPLER(unity_reflection_refraction, GetReflectionTextureSampler(UNITY_NEAR_CLIP_VALUE), i.worldPos.xy / i.worldPos.w);
                 // Combine the water color with the reflection and refraction
            float4 finalColor = lerp(waterColor, reflection, refraction.a);

            // Add some foam around the edges
            float2 foam = 1 - smoothstep(0.99, 1, length(uv - 0.5));
            finalColor.rgb += foam * 0.5;

            // Output the final color
            return finalColor;
        }
        ENDCG
    }
}
FallBack "Diffuse"
}