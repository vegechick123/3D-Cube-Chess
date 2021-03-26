Shader "Unlit/TextureBlend"
{
    Properties
    {
        _BlendTexture ("BlendTexture", 2D) = "white" {}
        _PaperBrightColor ("PaperBrightColor", Color) = (1,1,1,1)
        _PaperDarkColor ("PaperDarkColor", Color) = (0,0,0,1)
        _BlendStrength ("BlendStrength", Float) = 1
        [HideInInspector]_MainTex ("Texture", 2D) = "white"{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            ZTest Always
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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

            sampler2D _BlendTexture;
            float _BlendStrength;
            float4 _PaperBrightColor;
            float4 _PaperDarkColor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 a = tex2D(_MainTex, i.uv);
                fixed4 b = tex2D(_BlendTexture, i.uv);
                fixed4 col ;//= lerp(tex2D(_MainTex, i.uv),tex2D(_BlendTexture, i.uv),_BlendStrength);                                
                col = a*lerp(_PaperBrightColor,_PaperDarkColor,b.r*_BlendStrength);
                return col;
            }
            ENDCG
        }
    }
}
