Shader "Unlit/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Resolution ("Resolution", Range(0, 0.3)) = 0.1
        _Offset ("Offset", Range(0, 0.7)) = 0.5
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
            float _Offset;
            int _Range;

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
                fixed4 col = tex2D(_MainTex, i.uv);
                for (int index = 0; index < _Range; ++index)
                {
                    col.rgb += tex2D(_MainTex, i.uv + float2(0, _Offset+index)   * _Resolution).rgb;  // Up
                    col.rgb += tex2D(_MainTex, i.uv + float2(_Offset+index, 0)   * _Resolution).rgb;  // Right
                    col.rgb += tex2D(_MainTex, i.uv + float2(0, -_Offset+index)  * _Resolution).rgb;  // Down
                    col.rgb += tex2D(_MainTex, i.uv + float2(-_Offset+index, 0)  * _Resolution).rgb;  // Left
                    
                    
                    col.rgb += tex2D(_MainTex, i.uv + float2(_Offset+index, _Offset+index)   * _Resolution).rgb;  // Top Right
                    col.rgb += tex2D(_MainTex, i.uv + float2(_Offset+index, -(_Offset+index))  * _Resolution).rgb;  // Bottom Right
                    col.rgb += tex2D(_MainTex, i.uv + float2(-(_Offset+index), -(_Offset+index)) * _Resolution).rgb;  // Bottom Left
                    col.rgb += tex2D(_MainTex, i.uv + float2(-(_Offset+index), _Offset+index)  * _Resolution).rgb;  // Top Left
                }
                col.rgb /= (1 + 4 + 4) * _Range; // 5*3 pixels sampled, so /5*3
                
                return col;
            }
            ENDCG
        }
    }
}
