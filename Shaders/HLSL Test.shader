Shader "Lit/Test Shaders/HLSL Test" // this is a test hlsl surface shader
{
    Properties
    {
        Albedo ("Texture", 2D) = "white" {}
        NormalTex ("Normal Texture", 2D) = "none" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
           
        }
    }
}
