using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace DxrLattice {

sealed class HexagonalLatticeBuilder : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject _prefab = null;
    [SerializeField] uint3 _repeats = math.uint3(7, 5, 20);
    [SerializeField] float _removalRate = 0.4f;
    [SerializeField] uint _seed = 1234;

    #endregion

    #region Builder functions

    static readonly int Seed1Key = Shader.PropertyToID("_Seed1");
    static readonly int Seed2Key = Shader.PropertyToID("_Seed2");

    Random _random;

    float3 GetTubeOrigin(int column, int row)
    {
        var p = math.float2(column, row) - _repeats.xy / 2;
        p.x += (row & 1) * 0.5f;
        p *= math.float2(3, 0.87f);
        return math.float3(p, 0);
    }

    void BuildTube(Transform parent, float3 origin)
    {
        var seed1 = _random.NextFloat();
        var seed2 = _random.NextFloat();

        var pos = (float3)parent.position + origin;
        var offs = math.float3(0, 0.87f, 0);

        var prop = new MaterialPropertyBlock();

        for (var i = 0; i < _repeats.z;)
        {
            for (var j = 0; j < 3; j++)
            {
                if (_random.NextFloat() < _removalRate) continue;

                var phi = ((i + j) % 3 - 1) * math.PI / 3;
                var rot = quaternion.AxisAngle(math.float3(0, 0, 1), phi);
                var opos = pos + math.mul(rot, offs);

                prop.SetFloat(Seed1Key, seed1 + phi);
                prop.SetFloat(Seed2Key, seed2 + phi);

                var go = Instantiate(_prefab, opos, rot, parent);
                go.GetComponentInChildren<Renderer>().SetPropertyBlock(prop);
            }
            pos.z += 1;
            i++;
        }
    }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // PRNG initialization
        _random = new Random(_seed);
        _random.NextUInt4();

        // Tube array
        for (var row = 0; row < _repeats.y; row++)
            for (var col = 0; col < _repeats.x; col++)
                BuildTube(transform, GetTubeOrigin(col, row));
    }

    #endregion
}

} // namespace DxrLattice
