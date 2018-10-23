Shader "Custom/SphereShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Rate("Rate",Range(-300,300)) = 1.0
		_Size("Size",Range(0,300)) = 1.0
		_Xamt("X amt",Range(-100,100)) = 0
		_Yamt("Y amt",Range(-100,100)) = 0
		_Xoffset("X offSet",Range(-100,100)) = 0
		_Yoffset("Y offset",Range(-100,100)) = 0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Rate;
		float _Size;
		float _Xamt;
		float _Yamt;
		float _Xoffset;
		float _Yoffset;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float4 col;
			col = (1-_Color)*sin(((IN.uv_MainTex.y )*_Yamt + _Yoffset +(IN.uv_MainTex.x)*_Xamt+ _Xoffset )*_Size+ _Time[0]*_Rate);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * (1 - col);
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
