using LKS.Extentions;
using LKS.GameElements;
using LKS.Managers;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LKS.States.PlatformStates
{
    public class SlidingState : PlatformState
    {
        private Action OnComplete;

        private Vector3 _initialPosition;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;

        private float _slidingDistance;

        public SlidingState(Platform platform, float distance, Action onComplete) : base(platform)
        {
            _slidingDistance = distance;
            _initialPosition = _platform.LocalPosition;
            _targetPosition = _initialPosition.AddOnAxis(Axis.Y, _slidingDistance);

            OnComplete = onComplete;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            _currentPosition = _platform.LocalPosition;
            _currentPosition.y += _slidingDistance / GameManager.SlidingSpeed * Time.deltaTime;
            _platform.LocalPosition = _currentPosition;

            if(_currentPosition.y > _targetPosition.y)
            {
                GameUpdateManager.RemoveUpdatable(this);
                _platform.LocalPosition = _targetPosition;
                OnComplete?.Invoke();
            }
        }
    }
}