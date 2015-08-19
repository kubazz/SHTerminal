Shader "Custom/BlitSHGUI" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "" {}
		_MenuTex("Menu (ARGB)", 2D) = "" {}
	}

	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE

#include "UnityCG.cginc"

	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	sampler2D _MenuTex;

	float _GlobalScreenWidth;
	float _GlobalScreenHeight; 

	float _GuiXOffset;
	float _GuiYOffset;

	float _DefaultScreenRatio;
	float _CurrentScreenRatio;
	float _DefaultScreenRatioToCurrentScreenRatioRatio;

	v2f vert(appdata_img v) {
		v2f o;
		UNITY_INITIALIZE_OUTPUT(v2f, o)
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
		o.uv = v.texcoord.xy;
		return o;
	}

	half4 frag(v2f i) : COLOR {
		//UNITY_INITIALIZE_OUTPUT(v2f, i) //DAFAQ!? initializing output break blending between scene and menu texture
        float sizeX = _GlobalScreenWidth;
        float sizeY = _GlobalScreenHeight;

		float2 new_uv;

		if(_DefaultScreenRatioToCurrentScreenRatioRatio <= 1.0f){
			new_uv.x = (i.uv.x / _DefaultScreenRatio) * _CurrentScreenRatio - _GuiXOffset;
			new_uv.y = i.uv.y;
		} else {
			new_uv.x = i.uv.x;
			new_uv.y = (i.uv.y / _CurrentScreenRatio) * _DefaultScreenRatio - _GuiYOffset;
		}


		half4 mainColor = tex2D(_MainTex, i.uv);
		half4 menuColor = tex2D(_MenuTex, new_uv);
		half4 result;

		//OVERLAY
		//if (mainColor.r < 0.5) {
		//	result = 2.0 * mainColor * menuColor;
		//} else {
		//	result = float4(1.0, 1.0, 1.0, 1.0) - 2.0 * (float4(1.0, 1.0, 1.0, 1.0) - menuColor) * (float4(1.0, 1.0, 1.0, 1.0) - mainColor);
		//}

		//SCREEN
		//result = float4(1.0, 1.0, 1.0, 1.0) - (float4(1.0, 1.0, 1.0, 1.0) - menuColor) * (float4(1.0, 1.0, 1.0, 1.0) - mainColor);

		result.rgb = mainColor.rgb*(1-menuColor.a) + menuColor.rgb*(menuColor.a);
		result.a = 1.0f;
		//half4 menuColor = tex2D(_MenuTex, new_uv);

		//half4 color = tex2D(_MainTex, new_uv) + menuColor * menuColor.a ;
		return result;
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
