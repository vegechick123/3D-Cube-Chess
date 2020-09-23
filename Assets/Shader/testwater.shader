Shader "Unlit/testwater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DeepColor("DeepColor",Color)=(0,0,1,1)
        _ShallowColor("ShallowColor",Color)=(0,0,0.1,1)
        _Power("Fade",Float)=1
        _H("Height",Float)=1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
                float3 worldPos :TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _DeepColor;
            float4 _ShallowColor;
            float _H;
            float _Power;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos =mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed2 xy=i.uv-fixed2(0.5,0.5);
                fixed3 cord = fixed3(xy,max(abs(xy.x),abs(xy.y)));
                if(abs(cord.z-_H)<0.05);
                    //cord.z=1;
                else
                    cord.z=0;
                float3 viewDir=normalize(i.worldPos-_WorldSpaceCameraPos);
                fixed4 col =
                lerp(_DeepColor,_ShallowColor,pow(cord.z,_Power));
                //fixed4(cord.z,cord.z,cord.z,0);//_DeepColor;//tex2D(_MainTex, i.uv);
                // apply fog
                return col;
            }
            ENDCG
        }
    }
}
