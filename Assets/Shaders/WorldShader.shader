// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "DepthEffect" 
{ 
	Properties 
	{ 
		_MainTex ("Base (RGB)", 2D) = "white" {} 
	} 
	SubShader 
	{ 
		Pass 
		{ 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"
         
             struct v2f 
             {
                 float4 pos  : POSITION;
                 float2 uv   : TEXCOORD0;
                 float3 wpos : TEXCOORD1;
                 float3 vpos : TEXCOORD2;
             };
             
             v2f vert( appdata_img v )
             {
                 v2f o;
                 o.pos = UnityObjectToClipPos(v.vertex);
                 float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                 o.wpos = worldPos;
                 o.vpos = v.vertex.xyz;
                 o.uv =  v.texcoord.xy;
                 return o;
             }
             
             float4 frag (v2f i) : COLOR
             {
             	return float4(abs(i.wpos.x / 100), 0, 0, 1);
             }
             ENDCG
         }
     } 
}