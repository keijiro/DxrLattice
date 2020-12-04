using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace DxrLattice {

[VolumeComponentMenu("Sky/Dark Sky"), SkyUniqueID(UNIQUE_ID)]
public class DarkSky : SkySettings
{
    const int UNIQUE_ID = 0x0e186f1;

    public ColorParameter starColor =
      new ColorParameter(Color.white, true, false, true);

    public FloatParameter horizonPower =
      new FloatParameter(10);

    public FloatParameter polarPower =
      new FloatParameter(10);

    public override System.Type GetSkyRendererType()
      => typeof(DarkSkyRenderer);

    public override int GetHashCode()
      => base.GetHashCode() * 23 +
           starColor.GetHashCode() +
           horizonPower.GetHashCode() +
           polarPower.GetHashCode();
}

} // namespace DxrLattice
