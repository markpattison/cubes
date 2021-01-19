float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float xAmbient;
float3 xLightPosition;

struct VertexShaderInput
{
    float4 Position	: SV_POSITION;
	float3 Normal : NORMAL;
    float4 Colour : COLOR0;
};

struct VertexToPixel
{
	float4 Position : SV_POSITION;
	float3 WorldPosition: TEXCOORD0;
	float3 Normal : NORMAL;
	float4 Colour : COLOR0;
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
	output.WorldPosition = mul(input.Position, xWorld);
	output.Normal = normal;
	output.Colour = input.Colour;

	return output;
}

PixelToFrame CubePS(VertexToPixel input)
{
	PixelToFrame output;

	float3 lightDirection = normalize(input.WorldPosition - xLightPosition);
	float3 normal = normalize(input.Normal);
	float lightingFactor = saturate(dot(normal, -lightDirection)) * 1.0 + xAmbient;

	output.Colour = float4(input.Colour.rgb * lightingFactor, input.Colour.a);

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
