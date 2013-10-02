/*
    Copyright Predictions Software 2012
*/

Shader "Image50Alpha50x" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGBA)", 2D) = "white" {}
		_MaskAlphaStart ("MaskAlphaStart", Range (0.0,1.0)) = 0.0
	}
	SubShader {
	   Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float2 uv_MainTex;
      	};
      	
		sampler2D _MainTex;
		float _MaskAlphaStart;
		fixed4 _Color;
		
		void surf (Input IN, inout SurfaceOutput o) {
		    float2 xy = float2(IN.uv_MainTex.x,  IN.uv_MainTex.y);
			float3 rgb = tex2D (_MainTex, xy).rgb;
			xy = float2(IN.uv_MainTex.x, _MaskAlphaStart + IN.uv_MainTex.y);
			float a = tex2D (_MainTex, xy).r;
				
			o.Albedo = rgb * _Color.rgb;
			o.Alpha = _Color.a * a;
		}
		ENDCG
	} 
}

