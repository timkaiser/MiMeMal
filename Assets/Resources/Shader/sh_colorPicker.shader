Shader "Custom/ColorPicker"
{
	Properties
	{
		_Value("Value", Range(0.0,1.0)) = 1.0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		Pass
		{
		CGPROGRAM
			#pragma vertex vs_colorPicker
			#pragma fragment fs_colorPicker
			#include "UnityCG.cginc"

			struct Vertex
			{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
			};

			float _Value;

			//pass through
			Vertex vs_colorPicker(appdata_base input)
			{
				Vertex output;
				output.pos = UnityObjectToClipPos(input.vertex);
				output.tex = input.texcoord;
				return output;
			}

			float4 fs_colorPicker(Vertex input) : SV_Target
			{
				//scale to [-1, 1], centered around origin
				float x = (input.tex.x - 0.5) * 2;
				float y = (input.tex.y - 0.5) * 2;
				//compute polar coords
				float saturation = sqrt((x * x) + (y * y));
				float hue = atan2(y, x) * (180 / 3.14159265f) + 180;

				//if outside cicle
				if (saturation > 1) discard;

				//convert to RGB color
				float rk = (5 + hue / 60) % 6;
				float r = _Value - _Value * saturation * max(min(min(rk, 4 - rk), 1), 0);
				float gk = (3 + hue / 60) % 6;
				float g = _Value - _Value * saturation * max(min(min(gk, 4 - gk), 1), 0);
				float bk = (1 + hue / 60) % 6;
				float b = _Value - _Value * saturation * max(min(min(bk, 4 - bk), 1), 0);

				return float4(r, g, b, 1);
			}

		ENDCG
		}
	}
}
