// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/Broccoli/SproutLabNormals"
{
    Properties {
        // three textures we'll use in the material
        _MainTex("Base texture", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _Cutoff  ("Cutoff", Float) = 0.5
        _IsLinearColorSpace ("Is Linear Color Space", Float) = 0
    }
    SubShader
    {
        Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
        LOD 200

        Cull Off
        Lighting Off
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            // exactly the same as in previous shader
            struct v2f {
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldTangent : TEXCOORD2;
                float3 worldBitangent : TEXCOORD3;
                float2 uv : TEXCOORD4;
                float4 color : COLOR;
                float4 pos : SV_POSITION;
            };

            // textures from shader properties
            sampler2D _MainTex;
            sampler2D _BumpMap;
            float _Cutoff;
            float _IsLinearColorSpace;

            float3 RotateAroundYInDegrees (float3 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return float3(mul(m, vertex.xz), vertex.y).xzy;
            }

            v2f vert (float4 vertex : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float2 uv : TEXCOORD0, float4 color : COLOR)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                float3 wNormal = UnityObjectToWorldNormal(normal);
                wNormal = RotateAroundYInDegrees (wNormal, 90.0);
                float3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                float tangentSign = tangent.w * unity_WorldTransformParams.w;
                float3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                /*
                o.tspace0 = float3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = float3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = float3(wTangent.z, wBitangent.z, wNormal.z);
                */
                o.uv = uv;
                o.color = color;

                o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                o.worldNormal = mul(unity_ObjectToWorld, float4(normal, 0)).xyz; 
                o.worldTangent = mul(unity_ObjectToWorld, float4(tangent.xyz, 0)).xyz;
                o.worldBitangent = cross(o.worldNormal, o.worldTangent) * tangent.w;

                return o;
            }
        
            float4 frag (v2f i) : COLOR
            {
                /*
                // same as from previous shader...
                float3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));

                tnormal.x *= -1.5;
                tnormal.y *= 1.5;
                tnormal.xy *= 0.5f;
                //tnormal.xy *= 1.5;
                //float3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv)) * 0.5 + 0.5;
                float4 talbedo = tex2D(_MainTex, i.uv);
                float3 worldNormal;
                
                worldNormal.z = dot(i.tspace0, tnormal);
                worldNormal.y = dot(i.tspace1, tnormal);
                worldNormal.x = dot(i.tspace2, tnormal);
                worldNormal = RotateAroundYInDegrees (worldNormal, 90.0);
                //worldNormal.xy *= 1.2;
                //worldNormal *= 1.5;
                worldNormal.y *= -1;

                float4 c = 0;
                c.rgb = worldNormal*0.5+0.5;
                */
                float3 worldNormal = UnpackNormal(tex2D(_BumpMap, i.uv));
                /*
                worldNormal.x *= -1.5;
                worldNormal.y *= 1.5;
                worldNormal.xy *= 0.5f;
                */
                // i.worldNormal = normalize(i.worldNormal.x * i.worldTangent + i.worldNormal.y * i.worldBitangent + i.worldNormal.z * i.worldNormal);
                worldNormal = normalize(worldNormal.x * -i.worldTangent + worldNormal.y * i.worldBitangent + worldNormal.z * i.worldNormal);
                worldNormal = RotateAroundYInDegrees (worldNormal, 90.0);
                float4 c = float4(worldNormal * 0.5 + 0.5, 1);

                float4 talbedo = tex2D(_MainTex, i.uv);
                
                c.a = talbedo.a;
                float4 vcol = i.color;
                clip(c.a * vcol.a - _Cutoff);
                if (_IsLinearColorSpace > 0)
                    c.rgb = pow(c.rgb, 2.2);
                return c;
            }
            ENDHLSL
        }
    }
}