using UnityEngine;
using UnityEngine.Events;

namespace DxrLattice {

sealed class BeatSynchedEvent : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] float _bpm = 120;
    [SerializeField] UnityEvent _event = null;

    #endregion

    #region Private members

    int GetBeatCountAt(float t) => (int)(t * _bpm / 60);

    #endregion

    #region MonoBehaviour implementation

    void Update()
    {
        var b1 = GetBeatCountAt(Time.time - Time.deltaTime);
        var b2 = GetBeatCountAt(Time.time);
        if (b1 != b2) _event.Invoke();
    }

    #endregion
}

} // namespace DxrLattice
