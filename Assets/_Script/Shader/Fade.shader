Shader "Custom/Fade" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Grayscale("Grayscale", Range(0, 1)) = 1.00
		_Fade("Fade", Range(0, 1)) = 1.00
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			half4 _Color;
			float _Fade;
			float _Grayscale;

			fixed4 frag (v2f_img i) : SV_Target
			{
				float2 uv = i.uv;
				fixed4 c = tex2D(_MainTex, uv);

				c = c * _Grayscale + Luminance(c) * (1 - _Grayscale);
			    c = c * _Fade + _Color * (1 - _Fade);

				return c;
			}

		ENDCG
		}
	}
}