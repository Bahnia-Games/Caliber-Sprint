// I have no idea what any of this means tbh lol
float4x4 World;
float4x4 WorldViewProjection;
float LightPower;
float LightAmbient;
float3 LightDir;
Texture xTexture;

sampler TextureSampler = sampler_state
{
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct PixelColorOut
{
    float4 Color : COLOR0;
};

struct SceneVertexToPixel
{
    float4 Position : POSITION;
    float4 TexCoords : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float4 Position3D : TEXCOORD2;
};

SceneVertexToPixel GrayscaleVertexShader(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
{
    SceneVertexToPixel Output = (SceneVertexToPixel) 0;
    Output.Position = mul(inPos, WorldViewProjection);
    Output.Normal = normalize(mul(inNormal, (float3x3) World));
    Output.Position3D = mul(inPos, World);
    Output.TexCoords = inTexCoords; 
    return Output;
}

PixelColorOut GrayscalePixelShader(SceneVertexToPixel SVPin)
{
    PixelColorOut Output = (PixelColorOut) 0;
    float4 baseColor = tex2D(TextureSampler, SVPin.TexCoords);
    float diffuseLightingFactor = saturate(dot(-normalize(LightDir), SVPin.Normal)) * LightPower;
    float4 trueColor = baseColor * (diffuseLightingFactor + LightAmbient);
    float grayScaleAverage = (trueColor.r + trueColor.g + trueColor.b) / 3.0f;
    Output.Color = float4(grayScaleAverage, grayScaleAverage, grayScaleAverage, trueColor.a);
    return Output;
}

technique GrayscaleObject
{
    pass pass0
    {
        VertexShader = compile vs_2_0 GrayscaleVertexShader();
        PixelShader = compile ps_2_0 GrayscalePixelShader();
    }
}