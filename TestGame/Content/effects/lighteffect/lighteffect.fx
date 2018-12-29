sampler s0;
texture lightMask;
sampler lightSampler = sampler_state { Texture = <lightMask>; };
float4 AmbientColor = float4(1, 1, 1, 1);
float4 NightColor = float4(0, 0, 0.1, 0.1);
float4 White = float4(1,1,1,1);
float AmbientIntensity = 1.0;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords: TEXCOORD0) : COLOR0
{   
    float4 ncolor = NightColor * (1.0 - AmbientIntensity);
    float4 color = tex2D(s0, coords);
    float4 lightColor = tex2D(lightSampler, coords);  
    return color * ((lightColor*(1-AmbientIntensity*0.8)) + AmbientColor * AmbientIntensity) + ncolor * (1.0-lightColor) * (1.0 - AmbientIntensity);
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}