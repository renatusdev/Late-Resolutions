Shader "Renatusdev/Wobble" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        [Header(Wobble Properties)] [Space(5)]
        _ZMask("Z Mask", Range(0, 1)) = 1
        _XOscillationAmplitude("X Oscillation Amplitude", Range(0, 1)) = 1
        _YOscillationAmplitude("Y Oscillation Amplitude", Range(0, 1)) = 1
        _XOscillationSpeed("X Oscillation Speed", Range(0, 1)) = 1
        _YOscillationSpeed("Y Oscillation Speed", Range(0, 1)) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 test : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _ZMask;
            fixed _XOscillationAmplitude;
            fixed _YOscillationAmplitude;
            fixed _XOscillationSpeed;
            fixed _YOscillationSpeed;

            fixed4 remap(float In, fixed2 InMinMax, fixed2 OutMinMax) {
                return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
            
            Interpolator vert (MeshData v)
            {
                Interpolator i;
                i.test = v.vertex;
                
                float zMask = clamp(_ZMask-remap(i.test.z, float2(-1, 1), float2(0, 1)), 0, 1);
                
                v.vertex.x += zMask * (_XOscillationAmplitude* 10) * sin(_Time.y * (_XOscillationSpeed*20));
                v.vertex.y += zMask * (_YOscillationAmplitude* 10) * cos(_Time.y * (_YOscillationSpeed*20));
                
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(i,i.vertex);
                return i;
            }
            
            fixed4 frag (Interpolator i) : SV_Target {
                fixed4 txtSample = tex2D(_MainTex, i.uv);
                fixed4 finalOut = txtSample * _Color;
                UNITY_APPLY_FOG(i.fogCoord, finalOut);
                
                return finalOut;
            }
            ENDCG
        }
    }
}
