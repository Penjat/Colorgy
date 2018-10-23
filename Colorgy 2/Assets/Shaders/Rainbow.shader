Shader "Custom/Rainbow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_NoiseTex ("Noise Texture", 2D) = "black" {}

		_Rate ("Rate",Float) = 20
		_Size ("Size",Float) = 20
		_Distortion ("Distortion",Float) = 20
		_XMod ("X Mod",Float) = 1
		_ZMod ("Z Mod",Float) = 1
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
		sampler2D _NoiseTex;
		float _Rate;
		float _Size;
		float _Distortion;
		float _XMod;
		float _ZMod;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float4 surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			float r,b,g,x,y,d;
			d = abs(IN.uv_MainTex.y- IN.uv_MainTex.x);
			float offD = tex2D(_NoiseTex, float2(IN.worldPos.x,IN.worldPos.z+_Time[0])/_Distortion).r;
			//rate = 20;

			r = abs(sin(_Time[0]*_Rate+(IN.worldPos.x*_XMod-IN.worldPos.z*_ZMod+offD)*_Size));
			b = abs(sin(_Time[0]*_Rate+1+(IN.worldPos.x*_XMod-IN.worldPos.z*_ZMod+offD)*_Size) );
			g = abs(sin(_Time[0]*_Rate-1+(IN.worldPos.x*_XMod-IN.worldPos.z*_ZMod+offD)*_Size));
		
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * float4(r,g,b,1);
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			return c;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
