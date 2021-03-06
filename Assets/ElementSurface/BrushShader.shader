﻿Shader "Unlit/BrushShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,1)
       
    }
    
    SubShader
    {
        
        Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        //Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {

            HLSLPROGRAM
            //#include "UnityCG.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"     
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 uv   : TEXCOORD1;
            };

            struct v2f
            {
                
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 scrPos : TEXCOORD1;
            };
            
            #pragma vertex vert
		    // This line defines the name of the fragment shader. 
            #pragma fragment frag
            sampler2D _MainTex;
            //sampler2D _RenderTexture;
            float4 _Color;
            float _Threshold;
            v2f vert(Attributes v)
            {
                float3 v0 = v.positionOS.xyz;

                v2f OUT;
                OUT.positionCS = TransformObjectToHClip(v.positionOS);
                OUT.scrPos=ComputeScreenPos(OUT.positionCS);
                OUT.uv=v.uv;
                return OUT;
            }
            // The fragment shader definition.            
            half4 frag(v2f i) : SV_Target
            {                
                float s = tex2D(_MainTex,i.uv).r;
                //float origin = tex2D(_RenderTexture,i.scrPos.xy/i.scrPos.w).r;
                //float4 backColor=;
                return _Color*max(s-_Threshold,0);
            }
		    ENDHLSL
        }
    }
}
