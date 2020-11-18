using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace DxrLattice {

sealed class LineController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject _prefab = null;
    [SerializeField] int _instanceCount = 2;
    [SerializeField] float2 _extent = math.float2(1, 1);
    [SerializeField] float _depth = 20;
    [SerializeField] float _velocity = 10;
    [SerializeField] float _repeatPoint = -10;

    #endregion

    #region Line segment instance

    static uint _seed = 327845;

    struct Instance
    {
        public GameObject Root;
        public Random Random;
        public float3 Position;
        public float Speed;
    }

    void InitializeInstance(ref Instance i)
    {
        i.Root = Object.Instantiate(_prefab, transform);
        i.Random = new Random(_seed++);
        i.Random.NextUInt4();
        ResetInstance(ref i);
    }

    void ResetInstance(ref Instance i)
    {
        var p = i.Random.NextFloat2(-_extent, _extent);
        i.Position = math.float3(p, _depth);
        i.Speed = i.Random.NextFloat(0.7f, 1);
    }

    void UpdateInstance(ref Instance i)
    {
        var z = i.Position.z + i.Speed * -_velocity * Time.deltaTime;

        if (z > _repeatPoint)
        {
            i.Position.z = z;
            i.Root.transform.position = i.Position;
        }
        else
        {
            ResetInstance(ref i);
        }
    }

    #endregion

    #region MonoBehaviour implementation

    Instance [] _instances;

    void Start()
    {
        _instances = new Instance [_instanceCount];
        for (var i = 0; i < _instances.Length; i++)
            InitializeInstance(ref _instances[i]);
    }

    void Update()
    {
        for (var i = 0; i < _instances.Length; i++)
            UpdateInstance(ref _instances[i]);
    }

    #endregion
}

} // namespace MirrorWorld
