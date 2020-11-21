using UnityEngine;
using Unity.Mathematics;

namespace DxrLattice {

sealed class SquareLatticeBuilder : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject _prefab = null;
    [SerializeField] uint3 _repeats = math.uint3(7, 5, 20);

    #endregion

    #region Builder functions

    float3 GetTubeOrigin(int column, int row)
    {
        var p = math.float2(column, row) - _repeats.xy / 2;
        return math.float3(p, row & 1);
    }

    void BuildTube(Transform parent, float3 origin)
    {
        // Position offsets for horizontal/vertical walls
        var p_h = math.float3(0, 0.5f, 0);
        var p_v = math.float3(0.5f, 0, 0);

        // Rotations for horizontal/vertical walls
        var q_h = quaternion.identity;
        var q_v = quaternion.RotateZ(math.PI / 2);

        var pos = (float3)parent.position + origin;

        for (var i = 0; i < _repeats.z;)
        {
            Instantiate(_prefab, pos - p_h, q_h, parent);
            Instantiate(_prefab, pos + p_h, q_h, parent);
            pos.z += 1;
            i++;

            Instantiate(_prefab, pos - p_v, q_v, parent);
            Instantiate(_prefab, pos + p_v, q_v, parent);
            pos.z += 1;
            i++;
        }
    }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        for (var row = 0; row < _repeats.y; row++)
            for (var col = row & 1; col < _repeats.x; col += 2)
                BuildTube(transform, GetTubeOrigin(col, row));
    }

    #endregion
}

} // namespace DxrLattice
