Shader "Custom/UnlitMotionBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,1,0,1)
        _GlowIntensity ("Glow Intensity", Float) = 1
        _BlurStrength ("Blur Strength", Float) = 1
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
            float4 _GlowColor;
            float _GlowIntensity;
            float _BlurStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Apply the glow
                col += _GlowColor * _GlowIntensity;

                // Apply the motion blur
                // This is a simple example, you'll need to modify this based on your object's velocity and direction
                float2 blurUV = i.uv;
                blurUV.x += _BlurStrength * sin(_Time.y); // Modify this line to use your object's velocity
                col += tex2D(_MainTex, blurUV) * 0.5; // Blending with the original color

                return col;
            }
            ENDCG
        }
    }
}
