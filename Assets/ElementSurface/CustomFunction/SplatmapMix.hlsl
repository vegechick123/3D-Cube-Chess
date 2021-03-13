#ifndef SPLATMAPMIX_INCLUDE
#define SPLATMAPMIX_INCLUDE
// void Test_float()
// {

// }
//sampler2D _Control;
void SplatmapAlbedoMix_float(float4 diffAlbedo0, float4 diffAlbedo1, float4 diffAlbedo2,float4 diffAlbedo3,float4 base,float4 mix,out float3 albedo)
{

    albedo = float3(0,0,0);
    float4 splatControl=mix;

    albedo +=diffAlbedo0.rgb * splatControl.rrr;
    albedo +=diffAlbedo1.rgb * splatControl.ggg;
    albedo +=diffAlbedo2.rgb * splatControl.bbb;
    albedo +=diffAlbedo3.rgb * splatControl.aaa;
    albedo +=base.rgb *(1-(dot(splatControl,splatControl)));
}
#endif