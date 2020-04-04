void blurEffect_float(Texture2D sprite, SamplerState Sampler, float2 UV, float slice, float Power, out float4 Out)
{
    if(Power <= 0)
    {
        Out = SAMPLE_TEXTURE2D(sprite, Sampler, UV);
        return;
    }
    
    float2 localUV;
    float step = 0;
    
    Out = float4(0,0,0,0);
    float4 sampleOut;
    
    for(int i=0; i<8; i++)
    {
        step += slice;
        
        localUV = float2(sin(step), cos(step)) * Power;
        
        localUV = UV + localUV;
        
        sampleOut = SAMPLE_TEXTURE2D(sprite, Sampler, localUV);
        sampleOut /= 8;
        
        Out += sampleOut;
    }               
}
