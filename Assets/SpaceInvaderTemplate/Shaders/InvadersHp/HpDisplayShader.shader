Shader "Unlit/HpDisplayShader"
{
    Properties
    {
        _MainTex ("FillColor", 2D) = "Red" {}
        _FillCol ("FillColor", Color) = (1, 0, 0, 1)
        _BgCol ("Background Color", Color) = (0, 0, 0, 1)
        _Percent ("Fill Percent", Range(0, 1)) = 1
        _Smoothness ("Smoothness", Range(0, 0.1)) = 0.05
         _Alpha ("Alpha", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
//                Cull Off
//                ZWrite On
//                ZTest LEqual
//                Blend One Zero
//                AlphaToMask On
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FillCol;
            float4 _BgCol;
            float _Percent;
            float _Smoothness;
            float _Alpha;
            
            // float sdRoundedBox( in float2 p, in float2 b, in float4 r )
            // {
            //     r.xy = (p.x>0.0)?r.xy : r.zw;
            //     r.x  = (p.y>0.0)?r.x  : r.y;
            //     float2 q = abs(p)-b+r.x;
            //     return min(max(q.x,q.y),0.0) + length(max(q,0.0)) - r.x;
            // }
            //
            // float sdBox( in float2 p, in float2 b )
            // {
            //     float2 d = abs(p)-b;
            //     return length(max(d,0.0)) + min(max(d.x,d.y),0.0) - 0.01;
            // }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed t = smoothstep(_Percent-_Smoothness, _Percent+_Smoothness, i.uv);

                fixed4 col = lerp(_FillCol, _BgCol, t);
                col.a = _Alpha;
                // col.a *= (sdBox(i.uv, float2(1, 1)) > 0 ? 0 : 1);
                // col.a *= (sdRoundedBox(i.uv, float2(1, 1), float4(0.2, 1.2, 1.2, 1.2)) > 0 ? 0 : 1);
                // col.a = 0;
                
                // if (i.uv.x < _Percent)
                // {
                //     col = _FillCol;
                // } else
                // {
                //     col = _BgCol;
                // }
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
