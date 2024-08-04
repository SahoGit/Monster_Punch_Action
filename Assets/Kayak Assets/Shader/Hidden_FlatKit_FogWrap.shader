Shader "Hidden/FlatKit/FogWrap" {
	Properties {
		_Near ("Near", Float) = 0
		_Far ("Far", Float) = 100
		_DistanceFogIntensity ("Distance Fog Intensity", Range(0, 1)) = 1
		_LowWorldY ("Low", Float) = 0
		_HighWorldY ("High", Range(0, 1)) = 0.25
		_HeightFogIntensity ("Height Fog Intensity", Range(0, 1)) = 1
		_DistanceHeightBlend ("Distance / Height Blend", Range(0, 1)) = 0.5
		[NoScaleOffset] _DistanceLUT ("Distance LUT", 2D) = "white" {}
		[NoScaleOffset] _HeightLUT ("Height LUT", 2D) = "white" {}
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
}