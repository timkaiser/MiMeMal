Shader "Custom/UVShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" /*"Queue" = "Geometry+1"*/}
		LOD 100

		Pass
		{
			/*Stencil {
				Ref 2
				Comp equal
				Pass replace
			}*/

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				return float4(i.uv,0,1);
			}
			ENDCG
		}
	}
}
