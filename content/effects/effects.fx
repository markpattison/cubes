float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float xAmbient;
float3 xLightPosition;

float xCubeIndex;
float xFaceTag;
float xCubeTag;

struct VertexShaderInput
{
    float4 Position	: SV_POSITION;
	float3 Normal : NORMAL;
    float4 Colour : COLOR0;
	float Tag : TEXCOORD0;
};

struct VertexToPixel
{
	float4 Position : SV_POSITION;
	float3 WorldPosition: TEXCOORD0;
	float3 Normal : NORMAL;
	float4 Colour : COLOR0;
	bool IsHighlighted: TEXCOORD1;
};

struct PixelToFrame
{
	float4 Colour   : COLOR0;
};

VertexToPixel CubeVS(VertexShaderInput input)
{
	VertexToPixel output;

	float4x4 preViewProjection = mul(xView, xProjection);
	float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

	float3 normal = normalize(mul(float4(input.Normal, 0.0), xWorld)).xyz;

	output.Position = mul(input.Position, preWorldViewProjection);
	output.WorldPosition = mul(input.Position, xWorld).rgb;
	output.Normal = normal;
	output.Colour = input.Colour;
	output.IsHighlighted = abs(xFaceTag - input.Tag) < 0.01 && abs(xCubeTag - xCubeIndex) < 0.01;

	return output;
}

PixelToFrame CubePS(VertexToPixel input)
{
	PixelToFrame output;

	float3 lightDirection = normalize(input.WorldPosition - xLightPosition);
	float3 normal = normalize(input.Normal);
	float lightingFactor = saturate(dot(normal, -lightDirection)) * 1.0 + xAmbient;
	float4 colour = float4(input.Colour.rgb * lightingFactor, input.Colour.a);

	output.Colour = input.IsHighlighted ? float4(1.0, 1.0, 1.0, 1.0) : colour;

	return output;
}

technique Cube
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 CubeVS();
		PixelShader = compile ps_4_0 CubePS();
	}
}

/// Picker

struct PickerVertexToPixel
{
	float4 Position : SV_POSITION;
	float2 Colour : COLOR0;	
};

struct PickerPixelToFrame
{
	float4 Colour   : COLOR0;
};

PickerVertexToPixel PickerVS(VertexShaderInput input)
{
	PickerVertexToPixel output;

	float4x4 preViewProjection = mul(xView, xProjection);
	float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

	output.Position = mul(input.Position, preWorldViewProjection);

	output.Colour.r = xCubeIndex / 8.0;
	output.Colour.g = input.Tag / 6.0;

	return output;
}

PickerPixelToFrame PickerPS(PickerVertexToPixel input)
{
	PickerPixelToFrame output;

	output.Colour.rg = input.Colour;
	output.Colour.b = 0.0;
	output.Colour.a = 1.0;

	return output;
}

technique Picker
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 PickerVS();
		PixelShader = compile ps_4_0 PickerPS();
	}
}
