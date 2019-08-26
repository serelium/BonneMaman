Shader "BonneMaman/BlurOutline"
{
    Properties
    {
		_Intensity("Blur Intensity", Range(0.0,1.0)) = 0.5
		_BlurRadius("Blur Radius", Range(0.0,20.0)) = 1
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
			Name "HORIZONTALBLUROUTLINE"

			ZWrite Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
			};

			float _BlurRadius;
			float _Intensity;
			float _OutlineWidth;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;

			v2f vert(appdata_base v)
			{
				v2f o;

				v.vertex *= _OutlineWidth;
				o.vertex = UnityObjectToClipPos(v.vertex);

				#if UNITY_UV_STARTS_AT_TOP
					float scale = -1;
				#else
					float scale = 1;
				#endif

				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				half4 texcol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				half4 texsum = half4(0, 0, 0, 0);

				#define GRABPIXEL(weight, kernalx) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernalx * _BlurRadius, i.uvgrab.y, i.uvgrab.z, i.uvgrab.w))) * weight

				texsum += GRABPIXEL(0.05, -4.0);
				texsum += GRABPIXEL(0.09, -3.0);
				texsum += GRABPIXEL(0.12, -2.0);
				texsum += GRABPIXEL(0.15, -1.0);
				texsum += GRABPIXEL(0.18, -0.0);
				texsum += GRABPIXEL(0.15, 1.0);
				texsum += GRABPIXEL(0.12, 2.0);
				texsum += GRABPIXEL(0.09, 3.0);
				texsum += GRABPIXEL(0.05, 4.0);

				texcol = lerp(texcol, texsum, _Intensity);
				return texcol;
			}
			ENDCG
		}

		GrabPass{}

        Pass
        {
			Name "VERTICALBLUROUTLINE"

			ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
            };

			float _BlurRadius;
			float _Intensity;
			float _OutlineWidth;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;

			v2f vert(appdata_base v)
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

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				half4 texcol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				half4 texsum = half4(0, 0, 0, 0);

				#define GRABPIXEL(weight, kernaly) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x, i.uvgrab.y + _GrabTexture_TexelSize.y * kernaly * _BlurRadius, i.uvgrab.z,  i.uvgrab.w))) * weight

				texsum += GRABPIXEL(0.05, -4.0);
				texsum += GRABPIXEL(0.09, -3.0);
				texsum += GRABPIXEL(0.12, -2.0);
				texsum += GRABPIXEL(0.15, -1.0);
				texsum += GRABPIXEL(0.18, -0.0);
				texsum += GRABPIXEL(0.15, 1.0);
				texsum += GRABPIXEL(0.12, 2.0);
				texsum += GRABPIXEL(0.09, 3.0);
				texsum += GRABPIXEL(0.05, 4.0);

				texcol = lerp(texcol, texsum, _Intensity);
				return texcol;
			}
            ENDCG
        }
    }
}
