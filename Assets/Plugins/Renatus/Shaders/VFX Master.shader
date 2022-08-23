Shader "Renatusdev/VFX Master"
{
    Properties
    {
        [Header(General)]
        [Space(5)]

        _Texture ("Texture", 2D) = "white" {}
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _Opacity ("Opacity", Range(0, 1)) = 1

        [Enum(UnityEngine.Rendering.BlendMode)]
        _SourceFactor ("Source Factor", Float) = 1

        [Enum(UnityEngine.Rendering.BlendMode)]
        _DestinationFactor ("Destination Factor", Float) = 1

        [Enum(UnityEngine.Rendering.CullMode)]
        _CullMode ("Cull", Float) = 2

        [Header(Scrolling)]
        [Space(5)]

        _XScrollDirection("Scrolling Direction X", Range(-1, 1)) = 0
        _YScrollDirection("Scrolling Direction Y", Range(-1, 1)) = 0
        _ScrollSpeed("Scroll Speed", Range(0, 1)) = 0

        [Header(Distortion)]
        [Space(5)]

        _DistortionTexture ("UV Distortion Texture", 2D) = "white" {}
        _DistortionScrollDirectionX("Distortion Scrolling Direction X", Range(-1, 1)) = 0
        _DistortionScrollDirectionY("Distortion Scrolling Direction Y", Range(-1, 1)) = 0
        _DistortionScrollSpeed("Distortion Scroll Speed", Range(0, 1)) = 0
        _DistortionIntensity("UV Distortion Magnitude", Range(0, 1)) = 0

        [Header(Displacement)]
        [Space(5)]
        _DisplacementTest ("Test Displacement Value", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        ZWrite Off
        Blend [_SourceFactor] [_DestinationFactor]
        Cull [_CullMode]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD1;
            };

            sampler2D _Texture;
            sampler2D _DistortionTexture;
            
            float4 _Texture_ST;
            float3 _Color;

            float _Opacity;

            float _XScrollDirection;
            float _YScrollDirection;
            float _ScrollSpeed;

            float _DistortionScrollDirectionX;
            float _DistortionScrollDirectionY;
            float _DistortionScrollSpeed;
            float _DistortionIntensity;

            float _DisplacementTest;

            Interpolator vert (MeshData v)
            {
                // Vertex Displacement (shrink/expand on variable-axis based off varible-axis?)

                Interpolator i;

                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = v.uv;
                i.color = v.color;
                return i;
            }

            float4 frag (Interpolator i) : SV_Target
            {   
                // return sin(i.uv.y * 8);

                // UV Scrolling
                float2 txtUV = float2(_XScrollDirection, _YScrollDirection);    // Define 2D Direction of UV offset.
                txtUV *= _Time.y * _ScrollSpeed * 4;                            // Multiply by velocity of UV offset.
                txtUV += i.uv;                                                  // Add actual 2D UV grid.

                // UV Distortion
                float2 distortionTextureUV = float2(_DistortionScrollDirectionX, _DistortionScrollDirectionY);
                distortionTextureUV *= _Time.y * _DistortionScrollSpeed * 2;
                distortionTextureUV += i.uv;
                float distortion = tex2D(_DistortionTexture, distortionTextureUV);

                // Lerp default UVs with distortion UVs.
                txtUV = lerp(txtUV, distortion, _DistortionIntensity);

                // Fire sample texture
                float txtSample = saturate(tex2D(_Texture, txtUV));
                
                // Grouping
                float3 txt = _Color * txtSample;

                txt *= i.color.xyz;                
                return float4(txt, txtSample * _Opacity * i.color.a);
            }
            ENDCG
        }
    }
}
