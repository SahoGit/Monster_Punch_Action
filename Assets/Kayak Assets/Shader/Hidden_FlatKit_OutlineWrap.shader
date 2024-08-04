Shader "Hidden/FlatKit/OutlineWrap" {
	Properties {
		_EdgeColor ("Outline Color", Vector) = (0.3137255,0.6509804,0.2784314,1)
		_Thickness ("Thickness", Range(0, 5)) = 0
		_ColorThresholdMin ("Min Color Threshold", Range(0, 1)) = 0
		_ColorThresholdMax ("Max Color Threshold", Range(0, 1)) = 0.25
		_DepthThresholdMin ("Min Depth Threshold", Range(0, 1)) = 0
		_DepthThresholdMax ("Max Depth Threshold", Range(0, 1)) = 0.25
		_NormalThresholdMin ("Min Normals Threshold", Range(0, 1)) = 0.5
		_NormalThresholdMax ("Max Normals Threshold", Range(0, 1)) = 1
		_FadeRangeStart ("Fade Range Start", Float) = 10
		_FadeRangeEnd ("Fade Range End", Float) = 30
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