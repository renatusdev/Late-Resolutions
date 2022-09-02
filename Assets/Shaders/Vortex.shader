Shader "Unlit/Vortex"
{
    Properties
    {
        _TornadoMaskA ("Tornado Mask A", 2D) = "white" {}
        _TornadoMaskB ("Tornado Mask A", 2D) = "white" {}
        [HDR] _PrimaryColor ("Primary Color", Color) = (1,1,1,1)
        [HDR] _SecondaryColor ("Secondary Color", Color) = (1,0,0,1)
        _ScrollingSpeedA ("Scrolling Speed A", Range(0,1)) = 1
        _ScrollingSpeedB ("Scrolling Speed B", Range(0,1)) = 1
        _TornadoSpeed ("Tornado Speed", Range(0,1)) = 1
        
        [Enum(UnityEngine.Rendering.BlendMode)] _SourceFactor ("Source Factor", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DestinationFactor ("Destination Factor", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend [_SourceFactor]       [_DestinationFactor]
        Cull Front

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
                float3 normal : NORMAL;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _TornadoMaskA;
            sampler2D _TornadoMaskB;
            float3 _PrimaryColor;
            float3 _SecondaryColor;
            float _ScrollingSpeedA;
            float _ScrollingSpeedB;
            float _TornadoSpeed;

            Interpolator vert (MeshData v)
            {
                Interpolator i;
                
                v.vertex.x += sin(v.vertex.z - _Time.y * _TornadoSpeed * 15) * v.uv.y;
                v.vertex.y -= cos(v.vertex.z - _Time.y * _TornadoSpeed * 15) * v.uv.y;
                
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = v.uv;
                UNITY_TRANSFER_FOG(i,i.vertex);
                return i;
            }

            fixed4 frag (Interpolator i) : SV_Target
            {
                float2 uvA = i.uv + (_Time.y * _ScrollingSpeedA);
                float2 uvB = i.uv + (_Time.y * _ScrollingSpeedB);
                
                float tornadoMaskA = tex2D(_TornadoMaskA, uvA);
                float tornadoMaskB = tex2D(_TornadoMaskB, uvB);

                float4 colorA = float4(_PrimaryColor * tornadoMaskA, tornadoMaskA);
                float4 colorB = float4(_SecondaryColor * tornadoMaskB, tornadoMaskB);

                float4 finalColor = saturate(colorA + colorB); 
                UNITY_APPLY_FOG(i.fogCoord, finalColor);

                return finalColor;
            }
            ENDCG
        }
    }
}
