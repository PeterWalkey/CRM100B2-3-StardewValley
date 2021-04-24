Shader "Unlit/SimpleWaterShader"
{
    Properties
    {
         _LowColor("LowColor", Color) = (1,1,1)
         _HighColor("HighColor", Color) = (1,1,1)
         _MainTex("_MainTex", 2D) = "white" 
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
            float3 _LowColor;
            float3 _HighColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float random(float x) {

                return frac(sin(x) * 10000.);

            }

            float noise(float2 p) {

                return random(p.x + p.y * 10000.);

            }

            float2 sw(float2 p) { return float2(floor(p.x), floor(p.y)); }
            float2 se(float2 p) { return float2(ceil(p.x), floor(p.y)); }
            float2 nw(float2 p) { return float2(floor(p.x), ceil(p.y)); }
            float2 ne(float2 p) { return float2(ceil(p.x), ceil(p.y)); }

            float GentleWaves(float2 p) {

                float2 interp = smoothstep(0., 1., frac(p));
                float s = lerp(noise(sw(p)), noise(se(p)), interp.x);
                float n = lerp(noise(nw(p)), noise(ne(p)), interp.x);
                return lerp(s, n, interp.y);

            }

            float WaveChurn(float2 p) {

                float x = 0.;
                x += GentleWaves(p);
                x += GentleWaves(p * 2.) / 2.;
                x += GentleWaves(p * 4.) / 4.;
                x += GentleWaves(p * 8.) / 8.;
                x += GentleWaves(p * 16.) / 16.;
                x /= 1. + 1. / 2. + 1. / 4. + 1. / 8. + 1. / 16.;
                return x;

            }

            float WaveMotion(float2 p) {

                float x = WaveChurn(p + _Time.y);
                float y = WaveChurn(p - _Time.y);
                return WaveChurn(p + float2(x, y));

            }

            float WaveSwell(float2 p) {

                float x = WaveMotion(p);
                float y = WaveMotion(p + 100.);
                return WaveMotion(p + float2(x, y));

            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float n = WaveSwell(uv * 6.);

                return float4(lerp(_LowColor, _HighColor, n), 1.);
         
            }
            ENDCG
        }
    }
}
