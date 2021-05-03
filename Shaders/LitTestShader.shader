Shader "Lit/Test Shaders/LitTestShader"
{
    Properties
    {
        _Albedo ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Transparenct ("Transparency", Range(0.0, 1.0)) = 1.0
        _Metallic ("Metallic", 2D) = "none" {}
        _Normal ("Normal", 2D) = "none" {}
        _Emission ("Emission", 2D) = "none" {}
        _Specular ("Specular", 2D) = "none" {}
        _Roughness ("Gloss", 2D) = "none" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
            #include "UnityCG.cginc"

            #pragma surface surf Standard fullforwardshadows
            #pragma target 3.0

            // vars

            sampler2D _Albedo;
            sampler2D _Metallic;
            sampler2D _Normal;
            sampler2D _Emission;
            sampler2D _Specular;
            sampler2D _Roughness;

            fixed4 _Tint;

            // structs

            struct Input 
            {
                float2 AlbedoUV;
            };

            void surf (Input i, inout SurfaceOutputStandard o)
            {
                fixed4 outColor = tex2D(_Albedo, i.AlbedoUV) * _Tint;

                o.Albedo = outColor.rgb;
            }

        ENDCG
    }
    FallBack "Diffuse"
}
