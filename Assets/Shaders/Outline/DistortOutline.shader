Shader "BonneMaman/DistortOutline"
{
    Properties
    {
		_DistorColor("Distort Color", Color) = (1,1,1,1)
		_BumpAmount("Distortion Amount", Range(0,128)) = 10
		_DistortTex("Distortion Texture", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_OutlineWidth("Outline Width", Range(0.1,10.0)) = 1.1
    }

    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
		}

		GrabPass{}

		Pass
		{
			Name "DISTORTOUTLINE"

			ZWrite Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
				float2 uvbump : TEXCOORD1;
				float2 uvmain : TEXCOORD2;
			};

			float4 _DistorColor;
			float _BumpAmount;
			sampler2D _DistortTex;
			float4 _DistortTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;

			float _OutlineWidth;

			v2f vert(appdata v)
			{
				v.vertex *= _OutlineWidth;
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				#if UNITY_UV_STARTS_AT_TOP
					float scale = -1;
				#else
					float scale = 1;
				#endif

				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;

				o.uvbump = TRANSFORM_TEX(v.texcoord, _BumpMap);
				o.uvmain = TRANSFORM_TEX(v.texcoord, _DistortTex);

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump)).rg;
				float2 offset = bump * _BumpAmount * _GrabTexture_TexelSize.xy;
				i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

				half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				half4 tint = tex2D(_DistortTex, i.uvmain) * _DistorColor;

				return col * tint;
			}
			ENDCG
		}
    }
}
