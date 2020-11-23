using UnityEngine;
using Klak.Math;

namespace DxrLattice {

class MaterialOverride
{
    static readonly int _seed1Key = Shader.PropertyToID("_Seed1");
    static readonly int _seed2Key = Shader.PropertyToID("_Seed2");

    MaterialPropertyBlock _props;

    public MaterialOverride(uint seed)
      => _props = new MaterialPropertyBlock();

    public void SetParameters(uint seed)
    {
        var hash = new XXHash(seed);
        _props.SetFloat(_seed1Key, hash.Float(0));
        _props.SetFloat(_seed2Key, hash.Float(1));
    }

    public void Apply(Renderer renderer)
      => renderer.SetPropertyBlock(_props);

    public void FindRendererAndApply(GameObject go)
      => go.GetComponentInChildren<Renderer>().SetPropertyBlock(_props);
}

} // namespace DxrLattice
