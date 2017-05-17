Shader "Custom/WarpShader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_WarpPos("Warp position", Float) = 0
		_Radius("Radius", Float) = 10
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _WarpPos;
		float _Radius;

		struct Input {
			float2 uv_MainTex;
			float3 localPos;
			float amt;
		};


		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float amount = clamp((1 / (_WarpPos - v.vertex.y))-0.5, 0, 1);

			float angle = atan2(v.vertex.x, v.vertex.z);
			float2 target = float2(sin(angle) * _Radius, cos(angle) * _Radius);

			v.vertex.xz = lerp(v.vertex.xz, target, amount);
			o.localPos = v.vertex.xyz;
			o.amt = amount;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {

			clip(_WarpPos - IN.localPos.y -0.5);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) + IN.amt * 2;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
