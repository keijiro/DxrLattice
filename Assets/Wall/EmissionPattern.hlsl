#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

void EmissionPattern_float
  (float2 uv, float3 normal, float time, out float3 output)
{
    float mask = abs(normalize(normal).y) > 0.8;
    float em = abs(snoise(float2(uv.x * 10, time * 1.1))) < 0.1;
    output = em * mask;
}
