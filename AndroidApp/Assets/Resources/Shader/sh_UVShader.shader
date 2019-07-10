/* HEADER:
 * This shader renders the scene displaying the objects UV coordinates.
 */

Shader "Custom/UVShader"
{
	Properties{}
	SubShader
	{
		Tags { "RenderType" = "Opaque"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			// Output of the vertex shader and input for the fragment shader
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			// Vertex Shader
			v2f vert (appdata_base v) {
				v2f o = { UnityObjectToClipPos(v.vertex), v.texcoord.xy };
				return o;
			}
			
			// Fragment Shader
			float4 frag (v2f i) : SV_Target {
				return float4(i.uv,0,1);
			}

			ENDCG
		}
	}
}
