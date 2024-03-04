using DG.Tweening;
using LKS.Data;
using LKS.Extentions;
using LKS.GameElements;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LKS.States.PlatformStates
{
    public class SlidingState : PlatformState
    {
#region Constants & Fields
        private Action OnComplete;
        private Tween _tween;

        private Vector3 _initialPosition;
        private Vector3 _targetPosition;

        private float _slidingDistance;
#endregion

#region Constructors
        public SlidingState(Platform platform, float duration, LevelGenerationData levelGenerationData, Action onComplete) : base(platform)
        {
            _slidingDistance = levelGenerationData.PlatformsDistance;

            _initialPosition = _platform.LocalPosition;
            _targetPosition = _initialPosition.AddOnAxis(Axis.Y, _slidingDistance);

            OnComplete = onComplete;

            _tween = _platform.transform.DOLocalMoveY(_targetPosition.y, duration).SetEase(Ease.Linear);
            _tween.OnUpdate(UpdateState);
            _tween.OnComplete(OnMovementCompleted);
            _tween.Pause();
        }
#endregion

#region Public Methods
        public override void OnEnter()
        {
            base.OnEnter();
            _tween.PlayForward();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            _platform.CheckIfEnable();
        }
#endregion

#region Private Methods
        private void OnMovementCompleted()
        {
            if (_platform != null)
            {
                _platform.OnIterationComplete?.Invoke(_platform);
            }

            if (OnComplete != null)
            {
                OnComplete.Invoke();
            }
        } 
#endregion
    }
}