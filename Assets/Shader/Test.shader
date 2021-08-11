// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test"
{
	Properties
	{
		_Gradient ("Texture", 2D) = "white" {}
		_Size ("Size", float) = 1
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

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldPos: TEXCOORD1;
				float4 localPos: TEXCOOR2;
			};

			sampler2D _Gradient;
            float _Height;
            float _Offset;

			v2f vert (appdata v)
			{
				v2f o;
				float height = sin(v.vertex.x * 1 + _Time[1]);
				//height = tex2Dlod(_MainTex, float4(v.vertex.x, v.vertex.y, 0, 0)).g;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.localPos = mul(unity_WorldToObject, o.worldPos);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_Gradient, float2(i.localPos.y / _Height + _Offset, 0));
				return col;
			}
			ENDCG
		}
	}
}
