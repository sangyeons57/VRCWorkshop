Shader "Unlit/CloseUpEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Distance Threshold", Float) = 5.0
        _FadeRange("Fade Range", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FadeRange;
            float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float dist = distance(i.worldPos, _WorldSpaceCameraPos);
                float alpha = saturate(1.0 - (dist - _Threshold) / _FadeRange);
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return fixed4(texColor.rgb, alpha);
            }
            ENDCG
        }
    }
}
