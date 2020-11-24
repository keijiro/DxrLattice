#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

void DynamicStripe_float
  (float position, float time, float width, float seed, out float output)
{
    output = abs(snoise(float2(position, seed * 300 + time))) < width;
}

void RandomBand_float
  (float position, float width, float seed, out float output)
{
    uint iseed = seed * 65536;
    position -= Hash(iseed);
    width *= Hash(iseed + 1);
    output = position > -width && position < width;
}
