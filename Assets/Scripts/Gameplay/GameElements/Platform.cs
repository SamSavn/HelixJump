using LKS.Extentions;
using LKS.Managers;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LKS.Gameplay
{
    public class Platform : GameElement
    {
#region Constants & Fields
        public event Action<bool> OnToggle;
#endregion

#region Serialized Fields
        [SerializeField] private PlatformSegment[] _segments;
#endregion

#region Properties
        public float Position => transform.position.y;
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
        public void Initialize(float randomizationFactor)
        {
            if (_segments == null || _segments.Length == 0)
            {
                _segments = GetComponentsInChildren<PlatformSegment>();
            }

            if (!randomizationFactor.IsInRange(0, 1f))
            {
                randomizationFactor = .5f;
            }

            for (int i = 0; i < _segments.Length; i++)
            {
                _segments[i].Initialize(this, i, Random.value <= randomizationFactor);
            }

            Toggle(GameManager.Tower.CanActivate(this));
        } 

        public void Toggle(bool enable)
        {
            OnToggle?.Invoke(enable);
        }
#endregion
    }
}