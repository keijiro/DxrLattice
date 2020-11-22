#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

void EmissionPattern_float
  (float2 uv, float time, float seed, out float output)
{
    output = abs(snoise(float2(uv.x * 10 + seed, time * 1.1))) < 0.1;
}
