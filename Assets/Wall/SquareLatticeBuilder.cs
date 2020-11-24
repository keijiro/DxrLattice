using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

namespace DxrLattice {

sealed class SquareLatticeBuilder : MonoBehaviour
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

        // Column - Row - Edge x2 - Depth
        for (var col = 0u; col < _extent.x; col++)
        {
            for (var row = 0u; row < _extent.y; row++)
            {
                // Tube origin
                var org = math.float2(col, row) - _extent.xy / 2;

                for (var i = 0u; i < 2u; i++)
                {
                    // Per-strip random seed
                    var seed = hash.UInt(col) + hash.UInt(row) + i;

                    // Per-strip random material properties
                    mat.SetParameters(seed + 500);

                    // Rotation
                    var rot = quaternion.RotateZ(i * math.PI / 2);

                    // Position
                    var pos = math.float3(i * 0.5f, (1 - i) * 0.5f, 0);
                    pos.xy += org;

                    for (var depth = 0u; depth < _extent.z; depth++)
                    {
                        // Random removal
                        if (hash.Float(seed + depth) < _removalRate) continue;

                        // Instantiation and material overriding
                        pos.z = depth;
                        mat.Apply(Instantiate(_prefab, pos, rot, parent));
                    }
                }
            }
        }
    }

    #endregion
}

} // namespace DxrLattice
