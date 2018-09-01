Shader "Lit/GrimeLitTransparent"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_GrimeTex("Grime Visual", 2D) = "white" {}
		[HideInInspector]_GrimeMask("Grime Mask", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "LightMode" = "ForwardBase" "Queue" = "Transparent" "RenderType" = "Transparent" }
		Zwrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "Lighting.cginc"

		// compile shader into multiple variants, with and without shadows
		// (we don't care about any lightmaps yet, so skip these variants)
#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
		// shadow helper functions and macros
#include "AutoLight.cginc"

		struct v2f
	{
		float2 uv : TEXCOORD0;
		SHADOW_COORDS(1) // put shadows data into TEXCOORD1
			fixed3 diff : COLOR0;
		fixed3 ambient : COLOR1;
		float4 pos : SV_POSITION;
	};
	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		half3 worldNormal = UnityObjectToWorldNormal(v.normal);
		half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
		o.diff = nl * _LightColor0.rgb;
		o.ambient = ShadeSH9(half4(worldNormal,1));
		// compute shadows data
		TRANSFER_SHADOW(o)
			return o;
	}

	float4 _Color;
	sampler2D _MainTex;
	float4 _MainTex_ST;
	sampler2D _GrimeTex;
	float4 _GrimeTex_ST;
	sampler2D _GrimeMask;
	float4 _GrimeMask_ST;

	fixed4 frag(v2f i) : SV_Target
	{
		float g = tex2D(_GrimeMask, i.uv).x;
		float4 gtx = tex2D(_GrimeTex, i.uv);
		g = g * gtx.a;
		fixed4 col = lerp(tex2D(_MainTex, i.uv) * _Color, gtx, g);
		// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
		fixed shadow = SHADOW_ATTENUATION(i);
		// darken light's illumination with shadow, keep ambient intact
		fixed3 lighting = i.diff * shadow + i.ambient;
		col.rgb *= lighting;
		col.a = .5;
		return col;
	}
		ENDCG
	}

		// shadow casting support
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}