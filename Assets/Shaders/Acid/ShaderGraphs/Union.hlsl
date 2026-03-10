// Core implementation (float)
void Union_float4(float4 Top, float4 Bottom, out float4 Out)
{
    float a = saturate(Top.a);
    Out.rgb = lerp(Bottom.rgb, Top.rgb, a);
    Out.a = saturate(Bottom.a + Top.a);
}

// Wrapper name Unity/ShaderGraph may try to call (float precision)
void Union_float4_float(float4 Top, float4 Bottom, out float4 Out)
{
    Union_float4(Top, Bottom, Out);
}

// Wrapper name Unity/ShaderGraph may try to call (half precision)
void Union_float4_half(half4 Top, half4 Bottom, out half4 Out)
{
    half a = saturate(Top.a);
    Out.rgb = lerp(Bottom.rgb, Top.rgb, a);
    Out.a = saturate(Bottom.a + Top.a);
}