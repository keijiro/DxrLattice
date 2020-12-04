using System.Linq;
using UnityEngine;

namespace DxrLattice {

sealed class MotionVectorLimitter : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] float _threshold = 1;

    #endregion

    #region MonoBehaviour implementation

    (Transform xform, Renderer render, Vector3 prev) [] _objects;

    void Start()
      => _objects =
           FindObjectsOfType<Renderer>().
           Select(r => (r.GetComponent<Transform>(), r, Vector3.zero)).
           ToArray();

    void LateUpdate()
    {
        var thresh = _threshold * _threshold;

        for (var i = 0; i < _objects.Length; i++)
        {
            var pos = _objects[i].xform.position;

            _objects[i].render.motionVectorGenerationMode =
              (pos - _objects[i].prev).sqrMagnitude > thresh
                ? MotionVectorGenerationMode.ForceNoMotion :
                  MotionVectorGenerationMode.Object;

            _objects[i].prev = pos;
        }
    }

    #endregion
}

} // namespace DxrLattice
