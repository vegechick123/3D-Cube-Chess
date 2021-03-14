#ifndef SPLATMAPMIX_INCLUDE
#define SPLATMAPMIX_INCLUDE
// void Test_float()
// {

// }
//sampler2D _Control;
void SplatmapAlbedoMix_float(float4 diffAlbedo0, float4 diffAlbedo1, float4 diffAlbedo2,float4 diffAlbedo3,float4 mix,float threshold,out float3 albedo)
{

    albedo = float3(0,0,0);
    float4 splatControl=mix;
    float t=max(splatControl.r,splatControl.g);
    t=max(t,splatControl.b);
    t=max(t,splatControl.a);
    t=max(t,threshold);
    splatControl=step(t,splatControl);
    albedo +=diffAlbedo0.rgb * splatControl.rrr;
    albedo +=diffAlbedo1.rgb * splatControl.ggg;
    albedo +=diffAlbedo2.rgb * splatControl.bbb;
    albedo +=diffAlbedo3.rgb * splatControl.aaa;

}
#endif