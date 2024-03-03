using LKS.Data;
using LKS.Extentions;
using LKS.Iterations;
using LKS.Managers;
using System;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

namespace LKS.GameElements
{
    public class Platform : GameElement, IIterable
    {
#region Constants & Fields
        public event Action<bool> OnToggle;
        public event Action<bool> OnEnable;

        private Tower _tower;

        private float _slidingDistance;
        private int _index;
#endregion

#region Serialized Fields
        [SerializeField] private Collider _fallZone;
        [SerializeField] private PlatformSegment[] _segments;
#endregion

#region Properties
        public IIterable Previous { get; set; }
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
        public void Initialize(int index, Tower tower, LevelGenerationData levelGenerationData)
        {
            _tower = tower;
            _index = index;
            _slidingDistance = levelGenerationData.PlatformsDistance;

            if (_segments == null || _segments.Length == 0)
            {
                _segments = GetComponentsInChildren<PlatformSegment>();
            }

            float randomizationFactor = GetRandomizationFactor(levelGenerationData);

            if (index == 0)
            {
                randomizationFactor = levelGenerationData.PlatformsMinRandomization;
            }

            for (int i = 0; i < _segments.Length; i++)
            {
                _segments[i].Initialize(this, ActivateSegment(_segments[i]));
            }

            SetActive(GameManager.CanActivatePlatform(this));

            bool ActivateSegment(PlatformSegment segment)
            {
                if (_index > 0)
                    return Random.value <= randomizationFactor;

                return !segment.IsInitialSegment(levelGenerationData.InitialSegmentAngleThreshold)
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

        public void SetEnabled(bool enabled)
        {
            _fallZone.enabled = enabled;
            OnEnable?.Invoke(enabled);
        }

        public void OnIteration()
        {
            Vector3 pos = LocalPosition;
            pos.y += _slidingDistance;
            LocalPosition = pos;

            SetActive(_tower.CanActivatePlatform(this));
            SetEnabled(LocalPosition.y <= 0);
        }
#endregion

#region Private Methods
        private float GetRandomizationFactor(LevelGenerationData levelGenerationData)
        {
            return Random.Range(levelGenerationData.PlatformsMinRandomization, levelGenerationData.PlatformsMaxRandomizationFactor);
        } 
#endregion
    }
}