using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

namespace DxrLattice {

sealed class LayeredPlaneBuilder : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject _prefab = null;
    [SerializeField] uint3 _extent = math.uint3(7, 5, 20);
    [SerializeField] float _removalRate = 0.4f;
    [SerializeField] uint _randomSeed = 1;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        var hash = new XXHash(_randomSeed);
        var mat = new MaterialOverride(_randomSeed);

        var parent = transform;
        var rot = quaternion.identity;

        // Column - Row - Depth
        for (var col = 0u; col < _extent.x; col++)
        {
            for (var row = 0u; row < _extent.y; row++)
            {
                // Per-strip parameters
                var seed = hash.UInt(col) + hash.UInt(row);
                mat.SetParameters(seed + 500);

                for (var depth = 0u; depth < _extent.z; depth++)
                {
                    // Random removal
                    if (hash.Float(seed + depth) < _removalRate) continue;

                    // Instance position
                    var p = math.float3(col, row, depth);
                    p.xy = p.xy - _extent.xy / 2 + 0.5f;

                    // Instantiation and material overriding
                    var go = Instantiate(_prefab, p, rot, parent);
                    mat.FindRendererAndApply(go);
                }
            }
        }
    }

    #endregion
}

} // namespace DxrLattice
