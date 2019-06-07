/* HEADER:
 * This is the main shader for paintable objects. It renders the object with normal map and projects the outlines of the components onto is.
 */

Shader "Custom/PaintShader" { //source https://docs.unity3d.com/Manual/SL-SurfaceShaderExamples.html
	
	Properties{
		_MainTex("Texture", 2D) = "white" {}								// Main Texture. This is also the texture that is drawn on
		_OriginalTexture("Original Texture", 2D) = "clear" {}				// Original texture of the object
		_TextureBlendValue("Texture Blend Value", Range(0.0, 1.0)) = 0.5	// Value that determines how much of the originaö texture can be seen
		_NormalMap("Normalmap", 2D) = "bump" {}								// Normalmap
		_ComponentOutline("Component Outline", 2D) = "clear" {}				// Outlines of the components
		_ComponentMask("Component Mask", 2D) = "black" {}					// Mask that shows what part of the object belongs to which component. Not used in this shader. is only here for data storage

	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert

		// Standart Surface shader with normal map:
		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
		};

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _ComponentOutline;

		sampler2D _OriginalTexture;
		float _TextureBlendValue;

		void surf(Input IN, inout SurfaceOutput o) {
			float4 outline = tex2D(_ComponentOutline, IN.uv_MainTex);
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * (1 - _TextureBlendValue) + tex2D(_OriginalTexture, IN.uv_MainTex).rgb * _TextureBlendValue; // blend with original texture
			o.Albedo = o.Albedo * (1 - outline.a) + outline.rgb * outline.a; // outline is added here
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
			
		}
		ENDCG
	}
	Fallback "Diffuse"
}