﻿// Upgrade NOTE: replaced 'glstate.matrix.modelview[0]' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.projection' with 'UNITY_MATRIX_P'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon/Basic Outline 2" {
    Properties {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (.002, 0.03)) = .005
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { Texgen CubeNormal }
    }
 
    SubShader {
        Tags { "RenderType"="Opaque" }
        UsePass "Toon/Basic/BASE"
        Pass {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members vertex,normal)
#pragma exclude_renderers d3d11
#pragma vertex vert
 
struct appdata {
    float4 vertex;
    float3 normal;
};
 
struct v2f {
    float4 pos : POSITION;
    float4 color : COLOR;
    float fog : FOGC;
};
uniform float _Outline;
uniform float4 _OutlineColor;
 
v2f vert(appdata v) {
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    float3 norm = mul ((float3x3)UNITY_MATRIX_MV, v.normal);
    norm.x *= UNITY_MATRIX_P[0][0];
    norm.y *= UNITY_MATRIX_P[1][1];
    o.pos.xy += norm.xy * _Outline;
   
    o.fog = o.pos.z;
    o.color = _OutlineColor;
    return o;
}
ENDCG
            //Color (0,0,0,0)
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
            SetTexture [_MainTex] { combine primary }
        }
    }
   
    Fallback "Toon/Basic"
}