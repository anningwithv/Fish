// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PTGuideShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	//_Mask("Base (RGB)", 2D) = "white" {}


	_Color("Tint", Color) = (1,1,1,1)
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
		_Width("Width", Float) = 0.1
		_Height("Height", Float) = 0.1
		_CenterX("CenterX", Float) = 0.5
		_CenterY("CenterY", Float) = 0.5
		_Shape("Shape", Int) = 1
		_Radio("Radio", Float) = 1.3
		_CheckEdge("CheckEdge", Float) = 0
		_Edge("Edge", Vector) = (1,1,1,1)

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_ALPHACLIP

	struct a2v
	{
		fixed2 uv : TEXCOORD0;
		half4 vertex : POSITION;
		float4 color    : COLOR;
	};

	fixed4 _Color;
	float _Width;
	float _Height;
	float _CenterX;
	float _CenterY;
	float _Shape;
	float _Radio;
	float _CheckEdge;
	fixed4 _Edge;

	struct v2f
	{
		fixed2 uv : TEXCOORD0;
		half4 vertex : SV_POSITION;
		float4 color    : COLOR;
	};

	sampler2D _MainTex;

	v2f vert(a2v i)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(i.vertex);
		o.uv = i.uv;

		o.color = i.color * _Color;
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		fixed2 uv = i.uv;
		half4 color = tex2D(_MainTex, uv) * i.color;

		if (_CheckEdge > 0.0)
		{
			if (uv.x < _Edge.x || uv.x > _Edge.z || uv.y < _Edge.y || uv.y > _Edge.w)
			{
				color.a = 0.6;
				return color;
			}
		}

		float minX = _CenterX - _Width;
		float maxX = _CenterX + _Width;

		float minY = _CenterY - _Height;
		float maxY = _CenterY + _Height;

		if (uv.x < minX || uv.x > maxX || uv.y < minY || uv.y > maxY)
		{
			color.a = 0.6;
		}
		else
		{
			if (_Shape == 1.0)
			{
				color.a = 0;
			}
			else if (_Shape == 2.0)
			{
				float x = uv.x - _CenterX;
				float y = uv.y - _CenterY;

				float dis = x * x + (y * y) * _Radio * _Radio;
				float rr = _Width * _Height * _Radio;

				if (dis < rr)
				{
					color.a = 0.0;
				}
				else
				{
					color.a = 0.6;
				}
			}
		}

		return color;
	}
		ENDCG
	}
	}
}
