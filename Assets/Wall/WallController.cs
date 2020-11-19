using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace DxrLattice {

sealed class WallController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject _prefab = null;
    [SerializeField] uint3 _repeats = math.uint3(7, 5, 20);
    [SerializeField] float _thrust = 1;

    #endregion

    #region Wall instances

    List<Transform> _xforms = new List<Transform>();

    float3 GetTubeOrigin(int column, int row)
      => math.float3(column - _repeats.x / 2, row - _repeats.y / 2, row & 1);

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
            var go1 = Instantiate(_prefab, pos - p_h, q_h, parent);
            var go2 = Instantiate(_prefab, pos + p_h, q_h, parent);

            pos.z += 1;
            i++;

            var go3 = Instantiate(_prefab, pos - p_v, q_v, parent);
            var go4 = Instantiate(_prefab, pos + p_v, q_v, parent);

            pos.z += 1;
            i++;

            _xforms.Add(go1.transform);
            _xforms.Add(go2.transform);
            _xforms.Add(go3.transform);
            _xforms.Add(go4.transform);
        }
    }

    void MoveWalls(float delta)
    {
        foreach (var xform in _xforms)
        {
            var p = xform.localPosition;
            p.z -= delta;
            if (p.z < 0) p.z += _repeats.z;
            xform.localPosition = p;
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

    void Update()
      => MoveWalls(_thrust * Time.deltaTime);

    #endregion
}

} // namespace MirrorWorld
