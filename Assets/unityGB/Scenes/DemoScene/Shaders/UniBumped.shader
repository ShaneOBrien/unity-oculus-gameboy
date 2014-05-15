/// BumpMap shader, by Takohi (http://www.takohi.com)
/// It works like a normal Bumped shader except that it also uses the Main Texture as normal map.
/// Good for adding some reliefs to an object with a grayscale texture. 

Shader "Takohi/UniBumped" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "black" {}
        _BumpStrength ("HeightMap strength", Range (0.03, 100)) = 80.0
    }

    SubShader {
      Tags { "RenderType" = "Opaque" }

      CGPROGRAM
      #pragma surface surf BlinnPhong

      struct Input {
          float2 uv_MainTex;
      };
      sampler2D _MainTex;
      uniform float4 _MainTex_TexelSize;
      uniform float _BumpStrength;

      void surf (Input IN, inout SurfaceOutput o) 
      {
            float3 normal = float3(0, 0, 1);
            half3 c;
            
            c = tex2D (_MainTex, IN.uv_MainTex);
            float heightSampleCenter = (1 - c / 3); // (c / 3) if you want dark color seeming closer
            
            c = tex2D (_MainTex, IN.uv_MainTex + float2(_MainTex_TexelSize.x, 0));
            float heightSampleRight = (1 - c / 3); // (c / 3) if you want dark color seeming closer
            
            c = tex2D (_MainTex, IN.uv_MainTex + float2(0, _MainTex_TexelSize.y));
            float heightSampleUp = (1 - c / 3); // (c / 3) if you want dark color seeming closer     

            float sampleDeltaRight = heightSampleRight - heightSampleCenter;
            float sampleDeltaUp = heightSampleUp - heightSampleCenter;

            normal = cross(
            float3(1, 0, sampleDeltaRight * _BumpStrength), 
            float3(0, 1, sampleDeltaUp * _BumpStrength));
	
	  		normal = normalize(normal);

            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;//*0.001 + normal*0.5 + 0.5;
            o.Gloss = 0.9;
            o.Specular = 1;
            o.Normal = normal;
      }
      ENDCG
	}
}