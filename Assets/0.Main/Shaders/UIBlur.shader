Shader "Custom/UIBlur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 0.01)) = 0.002
        
        // Cần thiết cho UI Masking
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord  : TEXCOORD0;
                fixed4 color    : COLOR;
            };

            sampler2D _MainTex;
            float _BlurSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Lấy mẫu màu từ 9 điểm xung quanh để tạo hiệu ứng mờ
                fixed4 col = fixed4(0,0,0,0);
                float2 uv = i.texcoord;

                col += tex2D(_MainTex, uv + float2(-_BlurSize, -_BlurSize)) * 0.05;
                col += tex2D(_MainTex, uv + float2(0, -_BlurSize)) * 0.1;
                col += tex2D(_MainTex, uv + float2(_BlurSize, -_BlurSize)) * 0.05;
                col += tex2D(_MainTex, uv + float2(-_BlurSize, 0)) * 0.1;
                col += tex2D(_MainTex, uv + float2(0, 0)) * 0.4; // Điểm trung tâm trọng số cao nhất
                col += tex2D(_MainTex, uv + float2(_BlurSize, 0)) * 0.1;
                col += tex2D(_MainTex, uv + float2(-_BlurSize, _BlurSize)) * 0.05;
                col += tex2D(_MainTex, uv + float2(0, _BlurSize)) * 0.1;
                col += tex2D(_MainTex, uv + float2(_BlurSize, _BlurSize)) * 0.05;

                return col * i.color;
            }
            ENDCG
        }
    }
}