Shader "HQ Material" 
{
	Properties 
	{
		_Color     ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpecMap ("Specularmap", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 400
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0
		//input limit (8) exceeded, shader uses 9
		//#pragma exclude_renderers d3d11_9x
				
		fixed4 _Color;
		fixed  _Shininess;
		
		sampler2D _MainTex;
		sampler2D _SpecMap;
		sampler2D _BumpMap;
	
		struct Input 
		{
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 tex  = tex2D(_MainTex, IN.uv_MainTex);
			fixed spec = tex2D(_SpecMap, IN.uv_MainTex).r;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));

			fixed4 c   = tex * _Color;
			o.Albedo   = c.rgb;
			o.Gloss    = _Shininess.xxxx * spec;
			o.Alpha    = c.a;
			o.Specular = spec * _SpecColor;
		}
		ENDCG
	}
	FallBack "Self-Illumin/Specular"
}

