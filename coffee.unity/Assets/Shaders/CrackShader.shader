Shader "Custom/CrackShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CrackAmount ("Crack Amount", Range(0, 1)) = 0
        _FadeAmount ("Fade Amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
            };

            sampler2D _MainTex;
            float _CrackAmount;
            float _FadeAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float crack = step(0.5, abs(i.uv.x - 0.5) - _CrackAmount);
                float alpha = 1.0 - _FadeAmount;
                fixed4 col = tex2D(_MainTex, i.uv) * (1.0 - crack);
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}
