#ifndef DC_WIREFRAME_CORE_INCLUDED
#define DC_WIREFRAME_CORE_INCLUDED

// Fix for Unity 5.4 upgrade
//UNITY_SHADER_NO_UPGRADE
#if UNITY_VERSION >= 540
	#define _Object2World unity_ObjectToWorld
	#define _Projector unity_Projector
	#define _ProjectorClip unity_ProjectorClip

	#define DC_WIREFRAME_PROJECTOR_VAR\
		uniform float4x4 unity_Projector;\
		uniform float4x4 unity_ProjectorClip;
#else
	#define DC_WIREFRAME_PROJECTOR_VAR\
		uniform float4x4 _Projector;\
		uniform float4x4 _ProjectorClip;
#endif

// Properties

uniform fixed4 _WireframeColor;
uniform float _WireframeSize;
uniform float _WireframeUV;
uniform sampler2D _WireframeMaskTex;
uniform float4 _WireframeMaskTex_ST;
uniform sampler2D _WireframeTex;
uniform float4 _WireframeTex_ST;
uniform float _WireframeTexAniSpeedX;
uniform float _WireframeTexAniSpeedY;
uniform float4 _WireframeEmissionColor;
uniform float _WireframeAlphaCutoff;

// Helpers

inline float DrawWireframeAA(float3 n, float width)
{
	float a = 1.0f;
//	half3 w = abs(ddx(n.rgb)) + abs(ddy(n.rgb));
	half3 w =fwidth(n.xyz);
//// AA
	#if _WIREFRAME_AA
	half ww = (width-1.0f);
	ww=(ww>1.0f)?ww:0.0f;
	half3 steps = smoothstep(w*ww,w*width,n.rgb);
	a = min(min(steps.x, steps.y), steps.z);
// NoAA
	#else

	half3 steps = smoothstep(w*width,w*width+0.000000000001,n.rgb);
	a = min(min(steps.x, steps.y), steps.z);
	#endif	 

	return a;
}

inline float DC_APPLY_WIREFRAME_COLOR_TEX(inout fixed3 col,float3 n,float2 uv)
{
	fixed4 tex = tex2D(_WireframeTex, uv);
	float w = DrawWireframeAA(n,_WireframeSize);
	w = (1.0f-w)*_WireframeColor.a*tex.a;
	#ifndef _EMISSION
	col.rgb = lerp(col.rgb,_WireframeColor.rgb*tex.rgb,w);
	#else
	col.rgb = lerp(col.rgb,_WireframeColor.rgb*tex.rgb+_WireframeEmissionColor.rgb,w);
	#endif
	
	return w;
}

inline float DC_APPLY_WIREFRAME_COLOR_TEX_ALPHA(inout fixed3 col,float3 n,float alpha, float2 uv)
{
	fixed4 tex = tex2D(_WireframeTex, uv);
	float w = DrawWireframeAA(n,_WireframeSize);
	
	// Calculate alpha cutoff	
	float cutoffAlpha = alpha;
	if(alpha<=_WireframeAlphaCutoff) cutoffAlpha=0.0f;
	w = (1.0f-w)*_WireframeColor.a*cutoffAlpha*tex.a;
	
	#ifndef _EMISSION
	col.rgb = lerp(col.rgb,_WireframeColor.rgb*tex.rgb,w);
	#else
	col.rgb = lerp(col.rgb,_WireframeColor.rgb*tex.rgb+_WireframeEmissionColor.rgb,w);
	#endif
		
	return w;
}

inline float DC_APPLY_WIREFRAME_COLOR_TEX_MASK(inout fixed3 col,float3 n, float2 uv, float2 uv2){
	fixed4 tex = tex2D(_WireframeTex, uv);
	#ifdef _WIREFRAME_ALPHA_MASK_INVERT
		fixed mask = 1.0f-tex2D(_WireframeMaskTex, uv2).a;
	#else
		fixed mask = tex2D(_WireframeMaskTex, uv2).a;
	#endif
	float w = DrawWireframeAA(n,_WireframeSize);

	// Calculate alpha cutoff	
	float cutoffAlpha = mask;
	if(mask<=_WireframeAlphaCutoff) cutoffAlpha=0.0f;
	w = (1.0f-w)*cutoffAlpha*_WireframeColor.a*tex.a;
	
	// w = (1.0f-w)*mask*_WireframeColor.a*tex.a;
	#ifndef _EMISSION
	col.rgb = lerp(col.rgb,_WireframeColor.rgb*tex.rgb,w);
	#else
	col.rgb = lerp(col.rgb,_WireframeColor.rgb*tex.rgb+_WireframeEmissionColor.rgb,w);
	#endif
	return w;

}
// Marcos

#if _WIREFRAME_VERTEX_COLOR
	#define DC_WIREFRAME_COORDS(idx1,idx2) fixed4 mass:COLOR;float4 vcolor:COLOR1;float4 wireframe_tex:TEXCOORD##idx1;
	#define DC_WIREFRAME_TRANSFORM_VERTEX_COLOR(o,v) o.vcolor=v.color;
	#define DC_WIREFRAME_APPLY_VERTEX_COLOR(col,i) col=col*i.vcolor;
#else
	#define DC_WIREFRAME_COORDS(idx1,idx2) fixed4 mass:COLOR;float4 wireframe_tex:TEXCOORD##idx1;
	#define DC_WIREFRAME_TRANSFORM_VERTEX_COLOR(o,v);
	#define DC_WIREFRAME_APPLY_VERTEX_COLOR(col,i) ;
#endif

#if defined(_WIREFRAME_DX11)
#define DC_WIREFRAME_TRANSFER_COORDS(o) o.mass=fixed4(1.0f,0.0f,0.0f,0.0f);\
	o.wireframe_tex.xy=TRANSFORM_TEX(((_WireframeUV==0.0f)?v.uv0:v.uv1),_WireframeTex)+half2(_WireframeTexAniSpeedX,_WireframeTexAniSpeedY)*_Time.y;\
	o.wireframe_tex.zw=TRANSFORM_TEX(v.uv0,_WireframeMaskTex);\
	DC_WIREFRAME_TRANSFORM_VERTEX_COLOR(o,v);
#else
#define DC_WIREFRAME_TRANSFER_COORDS(o) if(v.uv4.y==1.0f)o.mass=float4(1.0f,0.0f,0.0f,0.0f);else if(v.uv4.y==2.0f)o.mass=float4(0.0f,1.0f,0.0f,0.0f);else if(v.uv4.y==4.0f)o.mass=float4(0.0f,0.0f,1.0f,0.0f);\
	o.wireframe_tex.xy=TRANSFORM_TEX(((_WireframeUV==0.0f)?v.uv0:v.uv1),_WireframeTex)+half2(_WireframeTexAniSpeedX,_WireframeTexAniSpeedY)*_Time.y;\
	o.wireframe_tex.zw=TRANSFORM_TEX(v.uv0,_WireframeMaskTex);\
	DC_WIREFRAME_TRANSFORM_VERTEX_COLOR(o,v);
#endif

#if _WIREFRAME_ALPHA_TEX_ALPHA
	#define DC_APPLY_WIREFRAME(col,alpha,i) DC_WIREFRAME_APPLY_VERTEX_COLOR(col,i);float w=DC_APPLY_WIREFRAME_COLOR_TEX_ALPHA(col,i.mass,alpha,i.wireframe_tex.xy);//alpha=w;
#elif _WIREFRAME_ALPHA_TEX_ALPHA_INVERT
	#define DC_APPLY_WIREFRAME(col,alpha,i) DC_WIREFRAME_APPLY_VERTEX_COLOR(col,i);float w=DC_APPLY_WIREFRAME_COLOR_TEX_ALPHA(col,i.mass,1.0f-alpha,i.wireframe_tex.xy);alpha+=w;
#elif _WIREFRAME_ALPHA_MASK
	#define DC_APPLY_WIREFRAME(col,alpha,i) DC_WIREFRAME_APPLY_VERTEX_COLOR(col,i);float w=DC_APPLY_WIREFRAME_COLOR_TEX_MASK(col,i.mass,i.wireframe_tex.xy,i.wireframe_tex.zw);alpha+=w;
#elif _WIREFRAME_ALPHA_MASK_INVERT
	#define DC_APPLY_WIREFRAME(col,alpha,i) DC_WIREFRAME_APPLY_VERTEX_COLOR(col,i);float w=DC_APPLY_WIREFRAME_COLOR_TEX_MASK(col,i.mass,i.wireframe_tex.xy,i.wireframe_tex.zw);alpha+=w;
#else
	#define DC_APPLY_WIREFRAME(col,alpha,i) DC_WIREFRAME_APPLY_VERTEX_COLOR(col,i);float w=DC_APPLY_WIREFRAME_COLOR_TEX(col,i.mass,i.wireframe_tex.xy);alpha+=w;
#endif

#endif //DC_WIREFRAME_CORE_INCLUDED