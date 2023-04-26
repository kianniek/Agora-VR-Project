Shader "Hidden/Amazing Assets/Vertex Thickness Generator/Culling" 
{
    SubShader 
	{
        Tags{ "DisableBatching" = "True" }

		Cull Off 
        ZWrite On
		Fog {Mode Off}
		
        Pass 
		{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            
            float4 vert(float4 v:POSITION) : SV_POSITION
		    {
                return UnityObjectToClipPos (v);
            }

            fixed4 frag() : SV_TARGET 
			{
                return fixed4(0, 0, 0, 1);
            }

            ENDCG
        }
    }
}
