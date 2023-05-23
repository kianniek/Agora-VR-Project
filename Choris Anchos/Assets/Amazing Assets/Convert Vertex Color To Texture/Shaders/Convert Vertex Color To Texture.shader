Shader "Hidden/Amazing Assets/Convert Vertex Color To Texture/Baker"
{
	Properties
	{
		_MainTex ("", 2D) = "white" {}		
        _SecondaryTex ("", 2D) = "black" {}		
	}

	CGINCLUDE
	#include "UnityCG.cginc" 


    sampler2D _MainTex;
    sampler2D _SecondaryTex;
    sampler2D _TextureAlpha;

    float2 _Resolution;
    

	float4 frag4(v2f_img i) : SV_Target
	{
		float4 B = tex2D(_MainTex, i.uv + float2( 0, -1) * _Resolution);
		float4 D = tex2D(_MainTex, i.uv + float2(-1,  0) * _Resolution);
		float4 E = tex2D(_MainTex, i.uv + float2( 0,  0) * _Resolution);
		float4 F = tex2D(_MainTex, i.uv + float2( 1,  0) * _Resolution);
		float4 H = tex2D(_MainTex, i.uv + float2( 0,  1) * _Resolution);

		float4 BH = H;
		if (B.a > H.a)
			BH = B;

		float4 FD = D;
		if (F.a > D.a)
			FD = F;

		float4 BHFD = FD;
		if (BH.a > FD.a)
			BHFD = BH;

		float4 final = E;
		if (BHFD.a > E.a)
			final = BHFD;

		

		return final;
	}

	float4 frag8(v2f_img i) : SV_Target
	{
		float4 B = tex2D(_MainTex, i.uv + float2( 0, -1) * _Resolution);
		float4 D = tex2D(_MainTex, i.uv + float2(-1,  0) * _Resolution);
		float4 E = tex2D(_MainTex, i.uv + float2( 0,  0) * _Resolution);
		float4 F = tex2D(_MainTex, i.uv + float2( 1,  0) * _Resolution);
		float4 H = tex2D(_MainTex, i.uv + float2( 0,  1) * _Resolution);

		float4 I = tex2D(_MainTex, i.uv + float2(-1,  1) * _Resolution);
		float4 J = tex2D(_MainTex, i.uv + float2( 1,  1) * _Resolution);
		float4 K = tex2D(_MainTex, i.uv + float2(-1, -1) * _Resolution);
		float4 L = tex2D(_MainTex, i.uv + float2( 1, -1) * _Resolution);

		float4 BH = H;
		if (B.a > H.a)
			BH = B;

		float4 FD = D;
		if (F.a > D.a)
			FD = F;

		float4 BHFD = FD;
		if (BH.a > FD.a)
			BHFD = BH;


		float4 IL = L;
		if (I.a > L.a)
			IL = I;

		float4 JK = K;
		if (J.a > K.a)
			JK = J;

		float4 ILJK = JK;
		if (IL.a > JK.a)
			ILJK = IL;


		float4 T = BHFD;
		if (ILJK.a > BHFD.a)
			T = ILJK;


		float4 final = E;
		if (T.a > E.a)
			final = T;

		

		return final;
	}

	float4 frag_CopyAlpha(v2f_img i) : SV_Target
	{
		float4 c = float4(tex2D(_MainTex, i.uv).rgb, tex2D(_TextureAlpha, i.uv).a);
		

		return c;
	}

	ENDCG


	Subshader
    {
		//0
		Pass	
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag4

            ENDCG
        }


		//1
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag8

            ENDCG
        }
			 

		//2
		Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_CopyAlpha

            ENDCG
        }


		//3
		Pass
        {
            ZTest Off
            ZWrite Off
            Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment fragRGB
     
            void vert (inout float4 vertex:POSITION, inout float2 uv:TEXCOORD0, inout float4 color:COLOR)
            {
                float2 texcoord = uv.xy;
                texcoord.y = 1.0 - texcoord.y;
                texcoord = texcoord * 2.0 - 1.0;
                vertex = float4(texcoord, 0.0, 1.0);
            }
     
            float4 fragRGB (float4 vertex:POSITION, float2 uv:TEXCOORD0, float4 color:COLOR) : SV_TARGET
            {
                return float4(color.rgb, 1);
            }

            ENDCG
        }

		//4
		Pass
        {
            ZTest Off
            ZWrite Off
            Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment fragAlpha
     
            void vert (inout float4 vertex:POSITION, inout float2 uv:TEXCOORD0, inout float4 color:COLOR)
            {
                float2 texcoord = uv.xy;
                texcoord.y = 1.0 - texcoord.y;
                texcoord = texcoord * 2.0 - 1.0;
                vertex = float4(texcoord, 0.0, 1.0);
            }
     
            float4 fragAlpha (float4 vertex:POSITION, float2 uv:TEXCOORD0, float4 color:COLOR) : SV_TARGET
            {
                return float4(color.aaa, 1);
            }

            ENDCG
        }

		//5
		Pass
        {
            ZTest Off
            ZWrite Off
            Cull Off

            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment fragCombine
     
     
            float4 fragCombine (v2f_img i) : SV_TARGET
            {
                return float4(tex2D(_MainTex, i.uv).rgb, tex2D(_TextureAlpha, i.uv).r);
            }

            ENDCG
        }

		//6
		Pass
        {
            ZTest Off
            ZWrite Off
            Cull Off

            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment fragCombineMulti
     
     
            float4 fragCombineMulti (v2f_img i) : SV_TARGET
            {
                float4 c1 = tex2D(_MainTex, i.uv);
                float4 c2 = tex2D(_SecondaryTex, i.uv);

                return 1.0 - (1.0 - c1) * (1.0 - c2);
            }

            ENDCG
        }

        //7
		Pass
        {
            ZTest Off
            ZWrite Off
            Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment fragWireframe
     
            void vert (inout float4 vertex:POSITION, inout float2 uv:TEXCOORD0)
            {
                float2 texcoord = uv.xy;
                texcoord.y = 1.0 - texcoord.y;
                texcoord = texcoord * 2.0 - 1.0;
                
                vertex = float4(texcoord, 0.0, 1.0);
            }
     
            float4 fragWireframe (float4 vertex:POSITION) : SV_TARGET
            {
                return float4(1, 1, 0, 1);
            }

            ENDCG
        }
    }
}
