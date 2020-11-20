using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Jobs;

namespace DxrLattice {

sealed class BulkScroller : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] float _speed = 1;
    [SerializeField] float _depth = 10;

    #endregion

    #region Scroll job

    struct ScrollJob : IJobParallelForTransform
    {
        public float Delta;
        public float Depth;

        public void Execute(int index, TransformAccess xform)
        {
            var p = xform.localPosition;
            p.z -= Delta;
            if (p.z < 0) p.z += Depth;
            xform.localPosition = p;
        }
    }

    #endregion

    #region MonoBehaviour implementation

    TransformAccessArray _taa;

    void Start()
      => _taa = new TransformAccessArray(
           Enumerable.Range(0, transform.childCount).
           Select(i => transform.GetChild(i)).
           ToArray());

    void OnDestroy()
      => _taa.Dispose();

    void LateUpdate()
      => new ScrollJob
           { Delta = _speed * Time.deltaTime, Depth = _depth }
           .Schedule(_taa).Complete();

    #endregion
}

} // namespace DxrLattice
