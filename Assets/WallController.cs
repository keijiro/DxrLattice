using UnityEngine;
using Unity.Mathematics;

namespace DxrLattice {

sealed class WallController : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] Vector3Int _repeats = new Vector3Int(6, 5, 20);
    [SerializeField] float _thrust = 1;
    [SerializeField] float _spin = 0.1f;

    float _offset, _roll;

    void Start()
    {
        for (var row = 0; row < _repeats.y; row++)
            for (var col = row & 1; col < _repeats.x; col += 2)
                BuildTube(transform, GetTubeOrigin(col, row));
    }

    void Update()
    {
        _offset = (_offset + _thrust * Time.deltaTime) % 2;
        _roll += _spin * Time.deltaTime;

        transform.localPosition = new Vector3(0, 0, -_offset);
        transform.localRotation = quaternion.RotateZ(_roll);
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
            Instantiate(_prefab, pos + math.float3(0, -0.5f, i), q_h, parent);
            Instantiate(_prefab, pos + math.float3(0, +0.5f, i), q_h, parent);
            i++;

            Instantiate(_prefab, pos + math.float3(-0.5f, 0, i), q_v, parent);
            Instantiate(_prefab, pos + math.float3(+0.5f, 0, i), q_v, parent);
            i++;
        }
    }
}

} // namespace MirrorWorld
