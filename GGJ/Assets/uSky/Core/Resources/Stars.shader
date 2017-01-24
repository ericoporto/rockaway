// Note: This Star shader need to be inside Resources folder for mobile build!

Shader "Hidden/uSky/Stars" {
SubShader {
			Tags { "Queue"="Geometry+502" "RenderType"="Background" }

			Blend OneMinusDstAlpha  OneMinusSrcAlpha	// alpha 0
//			Blend OneMinusDstAlpha  SrcAlpha			// alpha 1
			ZWrite Off Fog { Mode Off }

	Pass{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest	

		uniform half StarIntensity;
		
		struct appdata_t {
			float4 vertex : POSITION;
			float4 ColorAndMag : COLOR;
			float2 texcoord : TEXCOORD;
		};
		
		struct v2f 
		{
			float4 pos : SV_POSITION;
			half4 Color : COLOR;
			half2 uv : TEXCOORD;
		};	
			
		float GetFlickerAmount(in float2 pos)
		{
			const float2 tab[8] = 
  			{
				float2(0.897907815,-0.347608525), float2(0.550299290, 0.273586675), float2(0.823885965, 0.098853070), float2(0.922739035,-0.122108860),
				float2(0.800630175,-0.088956800), float2(0.711673375, 0.158864420), float2(0.870537795, 0.085484560), float2(0.956022355,-0.058114540)
			};
		
			float2 hash = frac(pos.xy * 256);
			float index = frac(hash.x + (hash.y + 1) * (_Time.x * 2 + unity_DeltaTime.z)); // flickering speed
//			float index = frac(hash.x + (hash.y + 1) * (_Time.x * 2)); // flickering speed
			index *= 8;

			float f = frac(index)* 2.5; // multiply 2.5 to emphasize flickering effects
			int i = (int)index;

			return tab[i].x + f * tab[i].y;
		}	
		
		v2f vert(appdata_t v)
		{
			v2f OUT = (v2f)0;

			float3 t = v.vertex.xyz + _WorldSpaceCameraPos.xyz ; 
			OUT.pos = mul(UNITY_MATRIX_MVP, float4 (t, 1))  ;

			float appMag = 6.5 + v.ColorAndMag.w * (-1.44 -2.5);
			float brightness = GetFlickerAmount(v.vertex.xy) * pow(5.0, (-appMag -1.44)/ 2.5);
			
			OUT.Color = StarIntensity * float4(brightness * v.ColorAndMag.xyz, brightness );
			OUT.uv = 5 * v.texcoord.xy - 5 * float2(0.5, 0.5);
			
			return OUT;
		}

		half4 frag(v2f IN) : SV_Target
		{
			half2 distCenter = IN.uv.xy;
			half scale = exp(-dot(distCenter, distCenter));
			half3 colFinal = (IN.Color.xyz * scale + 5 * IN.Color.w * pow(scale, 10));
			return half4( colFinal,0);
		}
		ENDCG
	  }
	} 
}
