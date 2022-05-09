float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;

struct VertexShaderInput
{
    float4 Position	: SV_POSITION;
    float4 Colour : COLOR0;
};

struct VertexToPixel
{
	float4 Position : SV_POSITION;
	float4 Colour : COLOR0;
};

struct PixelToFrame
{
	float4 Colour   : COLOR0;
};

VertexToPixel PosColVS(VertexShaderInput input)
{
	VertexToPixel output;

	float4x4 preViewProjection = mul(xView, xProjection);
	float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

	output.Position = mul(input.Position, preWorldViewProjection);
	output.Colour = input.Colour;

	return output;
}

PixelToFrame PosColPS(VertexToPixel input)
{
	PixelToFrame output;

	output.Colour = float4(input.Colour);

	return output;
}

technique PosCol
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 PosColVS();
		PixelShader = compile ps_4_0 PosColPS();
	}
}
