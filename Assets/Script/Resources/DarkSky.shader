Shader "Hidden/HDRP/Sky/DarkSky"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Sky/SkyUtils.hlsl"

    float3 _StarColor;
    float3 _SkyParams;

    float4 Vertex(uint vertexID : SV_VertexID) : SV_POSITION
    {
        return GetFullScreenTriangleVertexPosition(vertexID, UNITY_RAW_FAR_CLIP_VALUE);
    }

    float4 RenderSky(float4 positionCS, float exposure)
    {
        float3 viewDirWS = GetSkyViewDirWS(positionCS.xy);

        float y = viewDirWS.y;
        float z = max(0, -viewDirWS.z);

        float3 color = _StarColor * _SkyParams.x * exposure;
        color *= pow(1 - abs(y), _SkyParams.y);
        color *= pow(abs(z), _SkyParams.z);

        return float4(ClampToFloat16Max(color), 1);
    }

    float4 FragmentBaking(float4 positionCS : SV_POSITION) : SV_Target
    {
        return RenderSky(positionCS, 1);
    }

    float4 FragmentRender(float4 positionCS : SV_POSITION) : SV_Target
    {
        return RenderSky(positionCS, GetCurrentExposureMultiplier());
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            ZWrite Off ZTest Always Blend Off Cull Off
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentBaking
            ENDHLSL
        }

        Pass
        {
            ZWrite Off ZTest LEqual Blend Off Cull Off
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentRender
            ENDHLSL
        }
    }

    Fallback Off
}
