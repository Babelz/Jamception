Shader "HQ Lighting Reflective Emissive" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_ParallaxMap ("Heightmap", 2D) = "black" {}
		_SpecularMap ("Specular", 2D) = "white" {}
		_EmissiveTex ("Emissive", 2D) = "black" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		//_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_Parallax ("Height", Range (0.00, 0.1)) = 0.02
		_GlossMultiplier ("Gloss Multiplier", Range( 0, 10 ) ) = 0
		
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
		_PolishTex ("Polish", 2D) = "black" {}	
		_PolishMultiplier ("Polish Multiplier", Range( 0, 1 ) ) = 0.1
		_PolishPower ("Polish Power", Range( 0, 10 ) ) = 1
	}
	SubShader 
	{ 
		Tags { "RenderType"="Opaque" }
		LOD 200
			
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0
		//input limit (8) exceeded, shader uses 9 
		//#pragma exclude_renderers d3d11_9x
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _ParallaxMap;
		sampler2D _SpecularMap;
		sampler2D _PolishTex;
		sampler2D _EmissiveTex;
		
		fixed4 _Color;
		//half _Shininess;
		float _Parallax;
		
		samplerCUBE _Cube;
		float _PolishMultiplier;
		float _PolishPower;
		float _GlossMultiplier;
		 
		struct Input 
		{
			float2 uv_MainTex;
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		}; 
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			float h = tex2D (_ParallaxMap, IN.uv_MainTex).r;
			float2 offset = ParallaxOffset (h, _Parallax, IN.viewDir);
			IN.uv_MainTex += offset;
			
			fixed4 emissive = tex2D( _EmissiveTex, IN.uv_MainTex);
			fixed polish = tex2D (_PolishTex, IN.uv_MainTex).r;
			fixed4 spec_tex = tex2D(_SpecularMap, IN.uv_MainTex);
			fixed4 col_tex = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = col_tex.rgb * _Color.rgb;
			o.Gloss = polish.r; //_Shininess * polish.r 
			o.Alpha = col_tex.a;
			o.Specular = spec_tex.r;
			o.Normal = UnpackNormal( tex2D(_BumpMap, IN.uv_MainTex) );
				
			float4 worldRefl;
			worldRefl.xyz = WorldReflectionVector (IN, o.Normal);
			worldRefl.w   = polish.r*_GlossMultiplier;
			fixed4 reflcol = texCUBEbias (_Cube, worldRefl);
			//fixed4 reflcol = texCUBE (_Cube, worldRefl.xyz);

			// Old
			o.Emission = emissive;
			o.Emission = lerp( o.Emission, reflcol, 0.25 * pow(polish,_PolishPower)*_PolishMultiplier  );
			
			o.Albedo = lerp( o.Albedo, reflcol, 0.75 * pow(polish,_PolishPower)*_PolishMultiplier  );
			// New
			
			
			
			//o.Albedo = lerp( o.Albedo, reflcol, pow(polish,_PolishPower)*_PolishMultiplier  );
			//o.Albedo   = lerp( o.Albedo,   reflcol, polish*_Polish );
			
			//o.Albedo = _Color.rgb;  
			//o.Emission = _Color.rgb;
			//reflcol *=  p;
			//o.Emission = reflcol.rgb * _ReflectColor.rgb;
			
		}
		ENDCG
	}
	//FallBack "HQ Lighting"
	//FallBack "Reflective/Bumped Specular"
}
