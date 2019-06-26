Shader "Custom/Slider"
{
	Properties
	{
		_HSVColor("HSVColor", Vector) = (0,0,1,1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		Pass
		{
		CGPROGRAM
			#pragma vertex vs_slider
			#pragma fragment fs_slider
			#include "UnityCG.cginc"

			struct Vertex
			{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
			};

			float4 _HSVColor;

			//pass through
			Vertex vs_slider(appdata_base input)
			{
				Vertex output;
				output.pos = UnityObjectToClipPos(input.vertex);
				output.tex = input.texcoord;
				return output;
			}

			float4 fs_slider(Vertex input) : SV_Target
			{
				float hue = _HSVColor.x * 360;
				float saturation = _HSVColor.y;
				float value = input.tex.y;

				//convert to RGB color
				float rk = (5 + hue / 60) % 6;
				float r = value - value * saturation * max(min(min(rk, 4 - rk), 1), 0);
				float gk = (3 + hue / 60) % 6;
				float g = value - value * saturation * max(min(min(gk, 4 - gk), 1), 0);
				float bk = (1 + hue / 60) % 6;
				float b = value - value * saturation * max(min(min(bk, 4 - bk), 1), 0);

				return float4(r, g, b, 1);
			}

		ENDCG
		}
	}
}
