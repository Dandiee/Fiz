struct VS_INPUT
{
	float2 Position : POSITION;
	float4 Color : COLOR;
};

struct PS_INPUT
{
	float4 Position	: SV_POSITION;
	float4 Color : COLOR;
};


cbuffer ShaderContantBuffer
{
	matrix WorldProjectionView;	
};

PS_INPUT VS_Main( VS_INPUT input)
{
	PS_INPUT output = (PS_INPUT)0;
	output.Position = mul(float4(input.Position.x, input.Position.y, 0, 1), WorldProjectionView);
	output.Color = input.Color;
	return output;
}


float4 PS_Main(PS_INPUT input) : SV_TARGET 
{
	return input.Color;
}


