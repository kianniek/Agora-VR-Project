Shader "Hidden/Amazing Assets/Vertex Thickness Generator/Ray"
{
    SubShader
    {
        ZTest Always       
         
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            
            #include "UnityCG.cginc"


            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            fixed4 frag (UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
            {
                return float4(0, 1, 0, 1);
            }
            ENDCG
        }
    }
}