/*
    Copyright Predictions Software 2012
*/

Shader "GreenScreen" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGBA)", 2D) = "white" {}
		_GreenLow ("_GreenLow", Range (0.0,1.0)) = 0.4
		_RedLow ("_RedLow", Range (0.0,1.0)) = 0.4
		_RedHigh ("_RedHigh", Range (0.0,1.0)) = 0.6
		_BlueLow ("_BlueLow", Range (0.0,1.0)) = 0.3
		_BlueHigh ("_BlueHigh", Range (0.0,1.0)) = 0.6
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
		fixed4 _Color;
		float _GreenLow,_RedLow,_RedHigh,_BlueLow,_BlueHigh;
		
		void surf (Input IN, inout SurfaceOutput o) {
		    float2 xy = float2(IN.uv_MainTex.x,  IN.uv_MainTex.y);
			float r = tex2D (_MainTex, xy).r;
			float g = tex2D (_MainTex, xy).g;
			float b = tex2D (_MainTex, xy).b;
			float a;
			
			if( g > _GreenLow && r > _RedLow && r < _RedHigh && b > _BlueLow && b < _BlueHigh ) {
       			 a=0.0;
   		    } else {
                a=1.0;
    		}
			o.Albedo = float3(r,g,b) * _Color.rgb;
			o.Alpha = a * _Color.a;
		}
		ENDCG
	} 
}

