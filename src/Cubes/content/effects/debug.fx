struct VertexShaderInput
{
    float4 Position      : SV_POSITION;
    float2 TextureCoords : TEXCOORD0;
};

struct VertexToPixel
{
	float4 Position   	 : SV_POSITION;
	float2 TextureCoords : TEXCOORD0;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

texture xDebugTexture;
sampler DebugTextureSampler = sampler_state { texture = <xDebugTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

VertexToPixel DebugVS(VertexShaderInput input)
{
	VertexToPixel Output = (VertexToPixel)0;

	Output.Position = input.Position;
	Output.TextureCoords = input.TextureCoords;

	return Output;
}

PixelToFrame DebugPS(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	Output.Color = tex2D(DebugTextureSampler, PSIn.TextureCoords);

	return Output;
}

technique Debug
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 DebugVS();
		PixelShader = compile ps_4_0 DebugPS();
	}
}