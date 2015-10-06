Shader "SUPERHOT/Text Shader" {
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_Color ("Text Color", Color) = (1,1,1,1)
		_Glitch1Start("Glitch 1 Start", Float) = 0.1
		_Glitch1End("Glitch 1 Width", Float) = 0.2
		_Glitch2Start("Glitch 2 Start", Float) = 0.75
		_Glitch2End("Glitch 2 End", Float) = 0.9
		_GlitchTex("Glitch Texture", 2D) = "grey" {}
	}

	SubShader {

		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}
		Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {	
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members vertex,texcoord,scrPos)
#pragma exclude_renderers d3d11 xbox360

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex;
				fixed4 color;
				float2 texcoord;
				float4 scrPos;
			};

			sampler2D _MainTex;
			sampler2D _GlitchTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _Color;
			uniform float _Glitch1Start;
			uniform float _Glitch1End;
			uniform float _Glitch2Start;
			uniform float _Glitch2End;
			
			float rand(float2 co){
  				return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
			}
			
			v2f vert (in appdata_full v, out v2f o)
			{

				UNITY_INITIALIZE_OUTPUT(v2f,o);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.vertex += rand(_SinTime.xx+v.texcoord.xy)/100.0;
				o.color 	= v.color * _Color;
				o.texcoord 	= TRANSFORM_TEX(v.texcoord,_MainTex);
				o.scrPos 	= ComputeScreenPos(o.vertex);
				//o.texcoord += rand(_SinTime.xx)/200.0 - (1.0/400.0);
				return o;
			} 

			fixed4 frag (v2f i) : COLOR
			{
				float2 wcoord 	= (i.scrPos.xy/i.scrPos.w);
				float4 col 		= i.color;
				
				
				
				if((wcoord.y > _Glitch1Start && wcoord.y < _Glitch1Start+_Glitch1End)){
					//i.texcoord.x += rand(_SinTime.xx+i.texcoord.xy)/100.0;
					i.texcoord.x += pow(((tex2D(_GlitchTex, i.texcoord).r - 0.5)*100 + rand(_SinTime.xx)*8) * ((wcoord.y - (_Glitch1Start))) * ((wcoord.y - (_Glitch1Start+_Glitch1End))), 2);
					i.texcoord.y += (tex2D(_GlitchTex, i.texcoord).g - 0.5)/80 * ((wcoord.y - (_Glitch1Start))) * ((wcoord.y - (_Glitch1Start+_Glitch1End))); 
				}	
				col.a 			*= tex2D(_MainTex, i.texcoord).a;
				
				//col.r			= wcoord.x;
				//col.b			= wcoord.y;
				return col;
			}
			ENDCG 
		}
	}
}
