Shader "Unlit/GrimeShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_GrimeTex("Grime Visual", 2D) = "white" {}
		[HideInInspector]_GrimeMask("Grime Mask", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "LightMode"="ForwardBase"}
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

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _GrimeTex;
			float4 _GrimeTex_ST;
			sampler2D _GrimeMask;
			float4 _GrimeMask_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float g = tex2D(_GrimeMask, i.uv).x;
				float4 gtx = tex2D(_GrimeTex, i.uv);
				g = g * gtx.a;
				fixed4 col = lerp(tex2D(_MainTex, i.uv) * _Color, gtx, g);
				return col;
			}
			ENDCG
		}
	}
}
