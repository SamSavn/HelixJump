using LKS.Data;
using LKS.Iterations;
using LKS.States;
using LKS.States.PlatformStates;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LKS.GameElements
{
    public class Platform : GameElement, IIterable
    {
#region Constants & Fields
        public event Action<bool> OnToggle;
        public event Action<bool> OnEnable;
        public event Action OnDispose;

        private LevelGenerationData _levelGenerationData;
        private StateMachine<PlatformState> _stateMachine;
        private Tower _tower;

        private float _slidingDistance;
        private int _index;
#endregion

#region Serialized Fields
        [SerializeField] private Collider _fallZone;
        [SerializeField] private PlatformSegment[] _segments;
#endregion

#region Properties
        public Action<IIterable> OnIterationComplete { get; set; }
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
            _levelGenerationData = levelGenerationData;
            _slidingDistance = levelGenerationData.PlatformsDistance;

            if (_segments == null || _segments.Length == 0)
            {
                _segments = GetComponentsInChildren<PlatformSegment>();
            }

            float randomizationFactor = GetRandomizationFactor();
            float obstaclesRandomizationFactor = GetObstaclesRandomizationFactor();

            if (index == 0)
            {
                randomizationFactor = levelGenerationData.PlatformsMinRandomizationFactor;
                obstaclesRandomizationFactor = 0;
            }

            for (int i = 0; i < _segments.Length; i++)
            {
                _segments[i].Initialize(this, ActivateSegment(_segments[i]), Random.value <= obstaclesRandomizationFactor);
            }

            _stateMachine = new StateMachine<PlatformState>(new IdleState(this, levelGenerationData.PlatformsDistance));

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
            _stateMachine.ChangeState(new SlidingState(this, _slidingDistance, OnSlideCompleted));            
        }

        public override void Dispose()
        {
            base.Dispose();
            OnDispose?.Invoke();
        }
#endregion

#region Private Methods
        private float GetRandomizationFactor()
        {
            return Random.Range(_levelGenerationData.PlatformsMinRandomizationFactor, _levelGenerationData.PlatformsMaxRandomizationFactor);
        }

        private float GetObstaclesRandomizationFactor()
        {
            return Random.Range(_levelGenerationData.ObstaclesMinRandomizationFactor, _levelGenerationData.ObstaclesMaxRandomizationFactor);
        }

        private void OnSlideCompleted()
        {
            _stateMachine.ChangeState(new IdleState(this, _levelGenerationData.PlatformsDistance));
        }
#endregion
    }
}