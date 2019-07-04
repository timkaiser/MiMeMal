Shader "Custom/ColorPicker"
{
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
				float alpha = 1;

				//if outside cicle
				if (saturation > 1.01) discard;
				if (saturation > 1.005) alpha = 0.8f;
				if (saturation > 1) alpha = 0.5f;
				if (saturation > 1.95) alpha = 0.3f;

				//convert to RGB color
				float rk = (5 + hue / 60) % 6;
				float r = 1 - 1 * saturation * max(min(min(rk, 4 - rk), 1), 0);
				float gk = (3 + hue / 60) % 6;
				float g = 1 - 1 * saturation * max(min(min(gk, 4 - gk), 1), 0);
				float bk = (1 + hue / 60) % 6;
				float b = 1 - 1 * saturation * max(min(min(bk, 4 - bk), 1), 0);

				return alpha * float4(r, g, b, 1) + (1-alpha) * float4(1, 1, 1, 1);
			}

		ENDCG
		}
	}
}
