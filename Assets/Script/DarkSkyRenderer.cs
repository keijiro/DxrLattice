using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace DxrLattice {

public class DarkSkyRenderer : SkyRenderer
{
    static readonly int StarColorID = Shader.PropertyToID("_StarColor");
    static readonly int SkyParamsID = Shader.PropertyToID("_SkyParams");
    static readonly int PC2ViewID =
      Shader.PropertyToID("_PixelCoordToViewDirWS");

    Material _material;
    MaterialPropertyBlock _props = new MaterialPropertyBlock();

    static Shader GetShader()
      => Resources.Load<Shader>("DarkSky");

    public override void Build()
      => _material = CoreUtils.CreateEngineMaterial(GetShader());

    public override void Cleanup()
      => CoreUtils.Destroy(_material);

    protected override bool Update(BuiltinSkyParameters builtinParams)
      => false;

    public override void RenderSky
      (BuiltinSkyParameters builtinParams,
       bool renderForCubemap, bool renderSunDisk)
    {
        var settings = builtinParams.skySettings as DarkSky;

        var skyParams = new Vector3(
          GetSkyIntensity(settings, builtinParams.debugSettings),
          settings.horizonPower.value, settings.polarPower.value);

        _props.SetColor(StarColorID, settings.starColor.value);
        _props.SetVector(SkyParamsID, skyParams);
        _props.SetMatrix(PC2ViewID, builtinParams.pixelCoordToViewDirMatrix);

        var pass = renderForCubemap ? 0 : 1;
        CoreUtils.DrawFullScreen
          (builtinParams.commandBuffer, _material, _props, pass);
    }
}

} // namespace DxrLattice
