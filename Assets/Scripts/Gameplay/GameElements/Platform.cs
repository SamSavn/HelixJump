using LKS.Data;
using LKS.Iterations;
using LKS.Managers;
using LKS.States;
using LKS.States.PlatformStates;
using System;
using System.Runtime.InteropServices;
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
        private Tower _tower;

        private StateMachine<PlatformState> _stateMachine;

        private int _index;
        private int _activeSegmentsStreak;

        private float _randomizationFactor;
        private float _obstaclesRandomizationFactor;

        private bool _activateSegment;
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
            _index = index;
            _tower = tower;
            _levelGenerationData = levelGenerationData;

            if (_segments == null || _segments.Length == 0)
            {
                _segments = GetComponentsInChildren<PlatformSegment>();
            }

            _randomizationFactor = GetRandomizationFactor();
            _obstaclesRandomizationFactor = GetObstaclesRandomizationFactor();

            if (index == 0)
            {
                _randomizationFactor = levelGenerationData.PlatformsMinRandomizationFactor;
                _obstaclesRandomizationFactor = 0;
            }

            _activeSegmentsStreak = 0;
            for (int i = 0; i < _segments.Length; i++)
            {
                _activateSegment = ActivateSegment(_segments[i]);

                if (_activateSegment)
                {
                    _activeSegmentsStreak++;
                }

                _segments[i].Initialize(this, _activateSegment, Random.value <= _obstaclesRandomizationFactor);
            }

            _stateMachine = new StateMachine<PlatformState>(new IdleState(this, levelGenerationData));            
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

        public void ToggleSegments()
        {
            OnToggle?.Invoke(!IsActive);
        }

        public void CheckIfEnable()
        {
            SetEnabled(Position.y < GameManager.Ball.Position.y);
        }

        public void OnIteration()
        {
            _stateMachine.ChangeState(new SlidingState(this, _tower.SlidingDuration, _levelGenerationData, OnSlideCompleted));
        }

        public override void Dispose()
        {
            base.Dispose();

            _stateMachine = null;

            OnDispose?.Invoke();
            base.Dispose();
        }
#endregion

#region Private Methods
        private bool ActivateSegment(PlatformSegment segment)
        {
            if (_index == 0 && segment.IsInitialSegment(_levelGenerationData.InitialSegmentAngleThreshold))
            {
                return true;
            }

            if (_activeSegmentsStreak >= _levelGenerationData.MaxSegmentsInRow)
            {
                _activeSegmentsStreak = 0;
                return false;
            }

            return Random.value <= _randomizationFactor;
        }
        
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
            _stateMachine.ChangeState(new IdleState(this, _levelGenerationData));
        }
#endregion
    }
}