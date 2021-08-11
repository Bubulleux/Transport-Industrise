Shader "Custom/MapShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Gradient ("Gradient", 2D) = "white" {}
		_Height ("Map Height", float) = 1
		_Noise ("Noise", 2D) = "black" {}
		_SizeX ("Map Size X", float) = 1001
		_SizeY ("Map Size Y", float) = 1001
		_Power ("Power", float) = 0.3
		_Offset ("Offset", float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
				float4 tangent: TANGENT;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos: TEXCOORD1;
				float4 localPos: TEXCOOR2;
				fixed4 diff : COLOR0;
			};
            
			sampler2D _MainTex;
            sampler2D _Gradient;
			sampler2D _Noise;
			float _Height;
			float _SizeX;
			float _SizeY;
			float _Power;
			float _Offset;

			
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.localPos = mul(unity_WorldToObject, o.worldPos);
				//o.vertex += float4(0, (tex2Dlod(_Noise, float4(o.worldPos.x / _SizeX, o.worldPos.z / _SizeY, 0, 0)).r - 0.5) * _Power * 2, 0 , 0);
				
				o.uv = v.uv;
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0;
				return o;
			}

			float4 frag (v2f i) : SV_Target
			{
				float4 colorHeight = tex2D(_Gradient, float2(i.localPos.y / _Height + _Offset, 0));
				float4 colorTexture = tex2D(_MainTex, i.uv);
				float4 color = colorTexture.a > colorHeight.a ? colorTexture : colorHeight;
				// color = (colorTexture * colorTexture.a + colorHeight * colorHeight.a) / (colorTexture.a + colorHeight.a);
				// color = colorHeight;
				color *= i.diff;
				return color;
			}
			ENDCG
		}
	}
}
