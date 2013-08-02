Shader "Unlit_VertexColor_Lightmap"

{

    Properties

    {

        _MainTex ("Base", 2D) = "white" {}

    }

 

    SubShader

    {

        Tags { "RenderType"="Opaque" }

 

        Pass

        {

            CGPROGRAM

                #include "UnityCG.cginc"

                #pragma vertex vert

                #pragma fragment frag

                #pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

 

                struct v2f

                {

                    half4 color : COLOR;

                    fixed4 pos : SV_POSITION;

                    fixed2 uv[2] : TEXCOORD0;

                };

           

                sampler2D _MainTex;

                fixed4 _MainTex_ST;

 

                #ifdef LIGHTMAP_ON

                fixed4 unity_LightmapST;

                sampler2D unity_Lightmap;

                #endif

 

                v2f vert(appdata_full v)

                {

                    v2f o;

                    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

                    o.uv[0] = TRANSFORM_TEX(v.texcoord, _MainTex);

                    

                    #ifdef LIGHTMAP_ON

                    o.uv[1] = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

                    #endif

 

                    o.color = v.color;

 

                    return o;

                }

        

                fixed4 frag(v2f i) : COLOR

                {

                    fixed4 c = tex2D(_MainTex, i.uv[0]) * i.color;

 

                    #ifdef LIGHTMAP_ON

                    c.rgb *= DecodeLightmap(tex2D(unity_Lightmap, i.uv[1]));

                    #endif

 

                    return c;

                }

            ENDCG

        }

    }

}