Shader "Unlit/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Resolution ("Resolution (Res/ResFactor)", Range(0, 100)) = 1
        _ResolutionFactor ("Resolution Factor (Res/ResFactor)", Integer) = 1500
        _Range ("Range", Range(1, 10)) = 3
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Resolution;
            int _Range;
            int _ResolutionFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;
                fixed res = _Resolution/_ResolutionFactor;
                col.a = tex2D(_MainTex, i.uv).a;
                for (int x = -_Range; x < _Range; ++x)
                {
                    for (int y = -_Range; y < _Range; ++y)
                    {
                        float2 offset = float2(x, y) * res;
                        col.rgb += tex2D(_MainTex, i.uv + offset).rgb;
                    }
                }
                col /= _Range*_Range*4;
                return col;
            }
            ENDCG
        }
    }
}
