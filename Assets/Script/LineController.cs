using Unity.Mathematics;
using Random = Unity.Mathematics.Random;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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
    [SerializeField] Gradient _spectrum = null;

    #endregion

    #region Line segment instances

    static uint _seed = 327453;

    struct Instance
    {
        public GameObject Node;
        public HDAdditionalLightData Light;
        public Material Material;
        public Random Random;
        public float3 Position;
        public float Speed;
    }

    void InitializeInstance(ref Instance i)
    {
        // Prefab instantiation
        i.Node = Object.Instantiate(_prefab, transform);

        // Component references for fast access
        i.Light = i.Node.GetComponentInChildren<HDAdditionalLightData>();
        i.Material = i.Node.GetComponentInChildren<MeshRenderer>().material;

        // Per-instance random number generator
        i.Random = new Random(_seed++);
        i.Random.NextUInt4();

        // Initial reset
        ResetInstance(ref i);
    }

    void ResetInstance(ref Instance i)
    {
        // Position reset
        var p = i.Random.NextFloat2(-_extent, _extent);
        i.Position = math.float3(p, _depth);

        // New random speed multiplier
        i.Speed = i.Random.NextFloat(0.5f, 1);

        // Random color selection from the spectrum
        var color = _spectrum.Evaluate(i.Random.NextFloat());

        // Light color
        i.Light.color = color;

        // Emissive mesh color
        i.Material.SetColor("_EmissiveColor", color.linear * 1000);
    }

    void UpdateInstance(ref Instance i)
    {
        // Simple linear motion
        var z = i.Position.z + i.Speed * -_velocity * Time.deltaTime;

        if (z > _repeatPoint)
        {
            // Position update
            i.Position.z = z;
            i.Node.transform.position = i.Position;
        }
        else
        {
            // Reset at the repeat point
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

} // namespace DxrLattice
