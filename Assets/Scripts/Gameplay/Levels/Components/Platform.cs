using LKS.Extentions;
using UnityEngine;

namespace LKS.Gameplay
{
    public class Platform : MonoBehaviour
    {
#region Serialized Fields
        [SerializeField] private PlatformSegment[] _segments;
#endregion

#region Unity Methods
        private void Reset()
        {
#if UNITY_EDITOR
            _segments = GetComponentsInChildren<PlatformSegment>();
#endif
        }
#endregion

#region Public Methods
        public void Randomize(float factor)
        {
            if (!factor.IsInRange(0, 1f))
            {
                factor = .5f;
            }

            for (int i = 0; i < _segments.Length; i++)
            {
                _segments[i].Toggle(Random.value <= factor);
            }
        } 
#endregion
    }
}