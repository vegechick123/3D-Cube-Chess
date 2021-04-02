void SampleGradient_float(float inputValue,
                   float4 color1,
                   float4 color2,
                   float4 color3,
                   float4 color4,
                   float4 color5,
                   float location1,
                   float location2,
                   float location3,
                   float location4,
                   out float4 outFloat)
{
    if(inputValue<location1)
    {
        outFloat = color1;
    }
    else if(inputValue<location2)
    {
        outFloat = color2;
    }
    else if(inputValue<location3)
    {
        outFloat = color3;
    }
    else if(inputValue<location4)
    {
        outFloat = color4;
    }
    else
    {
        outFloat = color5;
    }
}