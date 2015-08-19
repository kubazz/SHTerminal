Shader "XULM/postprocess/postDywanyEffect" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "" {}
		_GradientMapPower("_GradientMapPower",float)=0
		_GradientMap("GradientMap", 2D) = "" {}
		_GlobalPower("GlobalPower", float) = 0
	}
	

	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE

#include "UnityCG.cginc"

	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;

	float _FactorX;
	float _FactorY;
	float _GlobalScreenWidth;
	float _GlobalScreenHeight;
	float _RedPower;
	float _EnemyRedPower;
	float _Wyszarzenie;
	float _GlobalPower;
	
	float _GradientMapPower;
	sampler2D _GradientMap;
	
	float sizeX;
	float sizeY;
	float poziomCzerwieni;
	float srednia;
	float2 new_uv;
	half4 color;
	half4 sourceColor;

	v2f vert(appdata_img v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	half4 frag(v2f i) : COLOR {

		//new_uv.x = i.uv.x;
		//new_uv.y = i.uv.y;
		
		sourceColor = tex2D(_MainTex, i.uv);
		srednia = (sourceColor.r+sourceColor.g+sourceColor.b)*0.3333;
		
		//color.rgb = (1-_GradientMapPower)*color.rgb + (_GradientMapPower)*tex2D(_GradientMap,float2(srednia,0.5f));
		color.r = (1-_GradientMapPower)*sourceColor.r + (_GradientMapPower)*tex2D(_GradientMap,float2(sourceColor.r,0.5f)).r;
		color.g = (1-_GradientMapPower)*sourceColor.g + (_GradientMapPower)*tex2D(_GradientMap,float2(sourceColor.g,0.5f)).g;
		color.b = (1-_GradientMapPower)*sourceColor.b + (_GradientMapPower)*tex2D(_GradientMap,float2(sourceColor.b,0.5f)).b;

		return color*_GlobalPower + sourceColor*(1.0f-_GlobalPower);
	}

	ENDCG

	Subshader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog{ Mode off }

			CGPROGRAM
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma vertex vert
#pragma fragment frag
			ENDCG
		}

	}
	Fallback off
} // shader