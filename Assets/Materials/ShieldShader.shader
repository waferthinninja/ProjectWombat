﻿Shader "Custom/Shield"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_Angle("WidthAngle", Range(0,3.14)) = 0
		_Angle2("HeightAngle", Range(0,3.14)) = 0
	}

	SubShader
	{
		Blend One One
		ZWrite Off
		Cull Off

		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float3 objectPos : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.objectPos = v.vertex.xyz;
				
				return o;
			}
			
			fixed4 _Color;
			float _WidthAngle;
			float _HeightAngle;

			fixed4 frag(v2f i) : SV_Target
			{
				// clip where not within angles specified
				float diff = _WidthAngle - abs(atan2(i.objectPos.x, i.objectPos.z));
				float diff2 = _HeightAngle - abs(atan2(i.objectPos.y, i.objectPos.z));
				clip(diff);
				clip(diff2);

				// edge glow
				float glow = 1 / (min(diff, diff2) * 80);
				fixed4 glowColor = fixed4(lerp(_Color.rgb, fixed3(1, 1, 1),  pow(glow, 2)), 1);
				fixed4 col = _Color * _Color.a + glowColor * glow;
				return col;
			}
			ENDCG
		}
	}
}
	

