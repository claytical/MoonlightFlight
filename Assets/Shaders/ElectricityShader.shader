Shader "Custom/ElectricityShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Speed("Speed", Float) = 1.0
        _NoiseStrength("Noise Strength", Float) = 0.1
        _Amplitude("Amplitude", Float) = 0.1
        _GlowColor("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity("Glow Intensity", Float) = 1.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            fixed4 _Color;
            float _Speed;
            float _NoiseStrength;
            float _Amplitude;
            fixed4 _GlowColor;
            float _GlowIntensity;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_NoiseTex;
            };

            void surf(Input IN, inout SurfaceOutput o)
            {
                // Calculate noise
                float noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r * _NoiseStrength;

                // Calculate vertex position
                float time = _Time.y * _Speed;
                float offset = sin(time + IN.uv_MainTex.x * 10) * _Amplitude;
                float2 uv = IN.uv_MainTex + float2(offset, noise);

                // Sample texture
                fixed4 c = tex2D(_MainTex, uv);
                o.Albedo = c.rgb * _Color.rgb;

                // Add glow
                o.Emission = _GlowColor.rgb * _GlowIntensity;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
