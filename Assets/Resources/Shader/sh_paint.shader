Shader "Custom/PaintShader" { //source https://docs.unity3d.com/Manual/SL-SurfaceShaderExamples.html
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_NormalMap("Normalmap", 2D) = "bump" {}
		_ComponentOutline("Component Outline", 2D) = "clear" {}
		_ComponentMask("Component Mask", 2D) = "black" {}
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
		};

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _ComponentOutline;

		void surf(Input IN, inout SurfaceOutput o) {
			float4 outline = tex2D(_ComponentOutline, IN.uv_MainTex);
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * (1 - outline.a) + outline.rgb * outline.a;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
			
		}
		ENDCG
	}
	Fallback "Diffuse"
}