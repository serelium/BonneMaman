Shader "BonneMaman/DimensionCamera"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_LCDMap("LCD Map", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct FragIn
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 screen_uv : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _TimeView;
			float4 _TimeView_ST;

			sampler2D _LCDMap;
			float4 _LCDMap_ST;

            FragIn vert (VertIn v)
            {
				FragIn o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screen_uv = float3((o.vertex.xy + o.vertex.w) * 0.5, o.vertex.w);

                return o;
            }

            fixed4 frag (FragIn i) : SV_Target
            {
				float2 correctedScreenUV = i.screen_uv.xy / i.screen_uv.z;
				correctedScreenUV.y = 1 - correctedScreenUV.y;
				float4 col = tex2D(_TimeView, correctedScreenUV);
				float4 lcdCol = tex2D(_LCDMap, correctedScreenUV * 20.0);

				return saturate(col + lcdCol * 0.3);
            }

            ENDCG
        }
    }
}
