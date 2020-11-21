using UnityEngine;
using Unity.Mathematics;

namespace DxrLattice {

sealed class TriangularLatticeBuilder : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject _prefab = null;
    [SerializeField] uint3 _repeats = math.uint3(7, 5, 20);

    #endregion

    #region Builder functions

    float3 GetTubeOrigin(int column, int row)
    {
        var p = math.float2(column, row) - _repeats.xy / 2;
        p.x += (row & 1) * 0.5f;
        return math.float3(p, 0);
    }

    void BuildTube(Transform parent, float3 origin, int start)
    {
        var pos = (float3)parent.position + origin;
        var offs = math.float3(0, -0.29f, 0);

        for (var i = 0; i < _repeats.z;)
        {
            var phi = (i + start) * math.PI * 2 / 3;
            var rot = quaternion.AxisAngle(math.float3(0, 0, 1), phi);
            var opos = pos + math.mul(rot, offs);
            Instantiate(_prefab, opos, rot, parent);
            pos.z += 1;
            i++;
        }
    }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        for (var row = 0; row < _repeats.y; row++)
            for (var col = 0; col < _repeats.x; col++)
                BuildTube(transform, GetTubeOrigin(col, row), col % 3);
    }

    #endregion
}

} // namespace DxrLattice
