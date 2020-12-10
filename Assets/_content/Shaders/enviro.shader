Shader "Environment"
{
	Properties 
	{
_Diffuse("_Diffuse", 2D) = "gray" {}
_Normal("_Normal", 2D) = "bump" {}
_EnviroColor("_EnviroColor", Color) = (0.5895523,0.5895523,0.5895523,1)
_EnviroPower("_EnviroPower", Float) = 3
_Specular("_Specular", Color) = (0.7835821,0.7835821,0.7835821,1)
_Gloss("_Gloss", Range(0,1) ) = 1
_Cubemap("_Cubemap", Cube) = "black" {}

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 3.0


sampler2D _Diffuse;
sampler2D _Normal;
float4 _EnviroColor;
float _EnviroPower;
float4 _Specular;
float _Gloss;
samplerCUBE _Cubemap;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_Diffuse;
float2 uv_Normal;
float3 worldRefl;
INTERNAL_DATA

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input, o);
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D1=tex2D(_Diffuse,(IN.uv_Diffuse.xyxy).xy);
float4 Tex2D0=tex2D(_Normal,(IN.uv_Normal.xyxy).xy);
float4 UnpackNormal0=float4(UnpackNormal(Tex2D0).xyz, 1.0);
float4 Multiply1=Tex2D1.aaaa * _EnviroColor;
float4 WorldReflection0=float4( WorldReflectionVector (IN, UnpackNormal0), 1.0);
float4 TexCUBE0=texCUBE(_Cubemap,WorldReflection0);
float4 Pow1=pow(TexCUBE0,_EnviroPower.xxxx);
float4 Multiply2=Multiply1 * Pow1;
float4 Multiply0=Tex2D0.aaaa * _Specular;
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Tex2D1;
o.Normal = UnpackNormal0;
o.Emission = Multiply2;
o.Specular = _Gloss.xxxx;
o.Gloss = Multiply0;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}