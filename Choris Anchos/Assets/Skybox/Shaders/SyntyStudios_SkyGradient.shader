// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/SkyGradient"
{
	Properties
	{
		_ColorTop("Color Top", Color) = (0,1,0.7517242,0)
		_ColorBottom("Color Bottom", Color) = (0,1,0.7517242,0)
		_Offset("Offset", Float) = 0
		_Distance("Distance", Float) = 0
		_Falloff("Falloff", Range( 0.001 , 100)) = 0.001
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma instancing_options procedural:setup
		#pragma multi_compile GPU_FRUSTUM_ON __
		#include "VS_indirect.cginc"
		#pragma surface surf Unlit keepalpha noshadow nofog 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _ColorBottom;
		uniform float4 _ColorTop;
		uniform float _Offset;
		uniform float _Distance;
		uniform float _Falloff;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float clampResult40 = clamp( ( ( _Offset + ase_worldPos.y ) / _Distance ) , 0.0 , 1.0 );
			float4 lerpResult11 = lerp( _ColorBottom , _ColorTop , saturate( pow( clampResult40 , _Falloff ) ));
			o.Emission = lerpResult11.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}