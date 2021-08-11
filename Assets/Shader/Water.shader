Shader "Custom/Water"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 viewVector : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				float3 viewVector = mul(unity_CameraInvProjection, float4(v.uv * 2 - 1, 0, -1));
				o.viewVector = mul(unity_CameraToWorld, float4(viewVector,0));
                return o;
            }

			sampler2D _CameraDepthTexture;
            float4 frag (v2f i) : SV_Target
            {
                float nonLinearDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                float4 color = LinearEyeDepth(nonLinearDepth) * length(i.viewVector) / 10;
                return color;
            }
            ENDCG
        }
    }
}
