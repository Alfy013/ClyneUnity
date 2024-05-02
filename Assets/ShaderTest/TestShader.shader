Shader "Unlit/TestShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "black" {}
		_TintColor("TintColor", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TintColor;


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}


			fixed4 frag (v2f i) : SV_Target
			{
				float d = length(i.uv);
				fixed4 col = float4(pow((1 - d * 2) - abs((_CosTime.w + 0.5) / 2) * (1 - d) + (1 - d) * 0.5, 3), pow((1 - d * 2) - abs((_SinTime.w + 0.75) / 2) * (1 - d) + (1 - d) * 0.5, 3), pow((1 - d * 2) - abs((_CosTime.w + 1.0) / 2) * (1 - d) + (1 - d) * 0.5, 3), 1);
				/*float omd = 1 - d;
				fixed4 col = float4(pow(omd * 2 - abs((_SinTime.w + 0.5) / 2) * omd + omd * 0.5, 3), pow(omd * 2 - abs((_SinTime.w + 1.0) / 2) * omd + omd * 0.5, 3), pow(omd * 2 - abs((_SinTime.w + 1.5) / 2) * omd + omd * 0.5, 3), 1);*/
				return col;
			}
			ENDCG
		}
	}
}
