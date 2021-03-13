Shader "Custom/SurfaceFire"
{
	// The properties block of the Unity shader. In this example this block is empty
	// because the output color is predefined in the fragment shader code.
	Properties
	{
		_Width("Width",Float) = 1
		_Height("Height",Float) = 1
		_TextureSheet("TextureSheet",2D) = "white"{}
		_AnimationSpeed("AnimationSpeed",Float) = 1
		_HorizontalCount("HorizontalCount",int) = 1
		_VerticalCount("VerticalCount",int) = 1
	}

		// The HLSL code block. Unity SRP uses the HLSL language.
		HLSLINCLUDE
		// This line defines the name of the vertex shader. 
#pragma vertex vert
		// This line defines the name of the fragment shader. 
#pragma fragment frag
#pragma require geometry
#pragma geometry geom

#define GrassSegments 5 // segments per blade
#define GrassBlades 4 // blades per vertex

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

#pragma multi_compile_fog   

		// The Core.hlsl file contains definitions of frequently used HLSL
		// macros and functions, and also contains #include references to other
		// HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"     
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

		// The structure definition defines which variables it contains.
		// This example uses the Attributes structure as an input structure in
		// the vertex shader.
	struct Attributes
	{
		// The positionOS variable contains the vertex positions in object
		// space.
		float4 positionOS   : POSITION;
		float3 normal :NORMAL;
		float2 texcoord : TEXCOORD0;
		float4 color : COLOR;
	};

	struct v2g
	{
		float4 pos : SV_POSITION;
		float3 norm : NORMAL;
		float2 uv : TEXCOORD0;
		float3 color : COLOR;


	};
	int _HorizontalCount;
	int _VerticalCount;
	float _AnimationSpeed;
	float _Width;
	float _Height;
	uniform float3 _PositionMoving;

	v2g vert(Attributes v)
	{
		float3 v0 = v.positionOS.xyz;

		v2g OUT;
		OUT.pos = v.positionOS;
		OUT.norm = v.normal;
		OUT.uv = v.texcoord;
		OUT.color = v.color;
		//discard;
		return OUT;
	}

	struct g2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float4 color:TEXCOORD1;

	};

	float rand(float3 co)
	{
		return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
	}



	// per new grass vertex
	g2f GrassVertex(float3 vertexPos, float width, float height, float offset, float curve, float2 uv, float3x3 rotation, float3 faceNormal, float3 color, float3 worldPos) {
		g2f OUT;
		OUT.pos = TransformObjectToHClip(vertexPos + mul(rotation, float3(width, height, curve) + float3(0, 0, offset)));
		return OUT;
	}
	g2f ConstructVertexFromWorldSpace(float3 vertexPos,float2 uvPos) {
		g2f OUT;
		OUT.pos = TransformWorldToHClip(vertexPos);
		OUT.uv=uvPos;
		OUT.color=float4(0,0,0,1);
		return OUT;
	}
	// wind and basic grassblade setup from https://roystan.net/articles/grass-shader.html
	// limit for vertices
	[maxvertexcount(4)]
	void geom(point v2g IN[1], inout TriangleStream<g2f> triStream)
	{

			// Add just below the loop to insert the vertex at the tip of the blade.
			float3 center = TransformObjectToWorld(IN[0].pos);
			float3 normalDir =  SafeNormalize(GetCameraPositionWS()-center);
			// If _VerticalBillboarding equals 1, we use the desired view dir as the normal dir
			// Which means the normal dir is fixed
			// Or if _VerticalBillboarding equals 0, the y of normal is 0
			// Which means the up dir is fixed
			normalDir.y =normalDir.y * 1;
			normalDir = normalize(normalDir);
			// Get the approximate up dir
			// If normal dir is already towards up, then the up dir is towards front
			float3 upDir = abs(normalDir.y) > 0.999 ? float3(0, 0, 1) : float3(0, 1, 0);
			float3 rightDir = normalize(cross(upDir, normalDir));
			upDir = normalize(cross(normalDir, rightDir));
			
			float time = floor(_AnimationSpeed*_Time.x+2333*rand(center));
			float row = floor(time / _HorizontalCount);
			float column = time - row * _HorizontalCount;
				
//				half2 uv = float2(i.uv.x /_HorizontalAmount, i.uv.y / _VerticalAmount);
//				uv.x += column / _HorizontalAmount;
//				uv.y -= row / _VerticalAmount;
			float2 uvOffset = float2(1.0/_HorizontalCount,1.0/_VerticalCount);
			half2 leftBottomUV = half2(column/_HorizontalCount, -row/_VerticalCount);

			
			triStream.Append(ConstructVertexFromWorldSpace(center+_Width/2*rightDir+_Height*upDir,leftBottomUV+uvOffset));
			triStream.Append(ConstructVertexFromWorldSpace(center-_Width/2*rightDir+_Height*upDir,leftBottomUV+float2(0,uvOffset.y)));
			triStream.Append(ConstructVertexFromWorldSpace(center+_Width/2*rightDir,leftBottomUV+float2(uvOffset.x,0)));
			triStream.Append(ConstructVertexFromWorldSpace(center-_Width/2*rightDir,leftBottomUV));
	
	}

	ENDHLSL

		// color pass
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" }
		Blend SrcAlpha One
		ZWrite Off
		Pass
		{

			HLSLPROGRAM
			
			sampler2D _TextureSheet;
			//Blend SrcAlpha OneMinusSrcAlpha
			// The fragment shader definition.            
			half4 frag(g2f i) : SV_Target
			{
				half a=tex2D(_TextureSheet,i.uv).r; 
				return  half4(1,0,0,a);
				//return  half4(i.uv.y,0,0,0);
			}
			ENDHLSL
	}
	}
}