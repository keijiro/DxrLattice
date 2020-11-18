using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace DxrLattice {

sealed class WallController : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] Vector3Int _repeats = new Vector3Int(6, 5, 20);
    [SerializeField] float _thrust = 1;

    List<Transform> _transforms = new List<Transform>();

    void Start()
    {
        for (var row = 0; row < _repeats.y; row++)
            for (var col = row & 1; col < _repeats.x; col += 2)
                BuildTube(transform, GetTubeOrigin(col, row));
    }

    void Update()
    {
        var delta = -_thrust * Time.deltaTime;
        foreach (var tr in _transforms)
        {
            var p = tr.localPosition;
            p.z += delta;
            if (p.z < 0) p.z += _repeats.z;
            tr.localPosition = p;
        }
    }

    float3 GetTubeOrigin(int column, int row)
      => math.float3(column - _repeats.x / 2, row - _repeats.y / 2, row & 1);

    void BuildTube(Transform parent, float3 origin)
    {
        var q_h = quaternion.identity;
        var q_v = quaternion.RotateZ(math.PI / 2);
        var pos = (float3)parent.position + origin;

        for (var i = 0; i < _repeats.z;)
        {
            var go1 = Instantiate(_prefab, pos + math.float3(0, -0.5f, i), q_h, parent);
            var go2 = Instantiate(_prefab, pos + math.float3(0, +0.5f, i), q_h, parent);
            i++;

            var go3 = Instantiate(_prefab, pos + math.float3(-0.5f, 0, i), q_v, parent);
            var go4 = Instantiate(_prefab, pos + math.float3(+0.5f, 0, i), q_v, parent);
            i++;

            _transforms.Add(go1.transform);
            _transforms.Add(go2.transform);
            _transforms.Add(go3.transform);
            _transforms.Add(go4.transform);
        }
    }
}

} // namespace MirrorWorld
