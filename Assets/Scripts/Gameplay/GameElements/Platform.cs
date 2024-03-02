using LKS.Extentions;
using LKS.Helpers;
using LKS.Iterations;
using LKS.Managers;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LKS.GameElements
{
    public class Platform : GameElement, IIterable
    {
#region Constants & Fields
        public event Action<bool> OnToggle;
        private int _index;
#endregion

#region Serialized Fields
        [SerializeField] private Collider _fallZone;
        [SerializeField] private PlatformSegment[] _segments;
#endregion

#region Properties
        public IIterable Next { get; set; }
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
        public void Initialize(int index, float randomizationFactor)
        {
            _index = index;

            if (_segments == null || _segments.Length == 0)
            {
                _segments = GetComponentsInChildren<PlatformSegment>();
            }

            if (index == 0)
            {
                randomizationFactor = .3f;
            }
            if (!randomizationFactor.IsInRange(0, 1f))
            {
                randomizationFactor = .5f;
            }

            for (int i = 0; i < _segments.Length; i++)
            {
                _segments[i].Initialize(this, ActivateSegment(_segments[i]));
            }

            SetActive(GameManager.Tower.CanActivatePlatform(this));

            bool ActivateSegment(PlatformSegment segment)
            {
                if (_index > 0)
                    return Random.value <= randomizationFactor;

                return !segment.IsInitialSegment()
                            ? Random.value <= randomizationFactor
                            : true;
            }
        }

        public override void SetActive(bool active)
        {
            _fallZone.enabled = active;
            OnToggle?.Invoke(active);
            base.SetActive(active);
        }

        public void OnIteration()
        {
            Vector3 pos = Position;
            pos.y += 10f;
            Position = pos;
        }
#endregion
    }
}