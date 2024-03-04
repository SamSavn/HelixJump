using DG.Tweening;
using LKS.GameElements;
using LKS.Managers;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class FallingState : BallState
    {
        private Action OnComplete;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;
        private Tween _tween;
        private float _distance;

#region Constructors
        public FallingState(Ball ball, Action onComplete) : base(ball)
        {
            OnComplete = onComplete;
        }
#endregion

#region Public Methods
        public override void OnEnter()
        {
            base.OnEnter();

            _ball.Rigidbody.Sleep();
            _currentPosition = _ball.Position;
            _targetPosition = _currentPosition;
            _targetPosition.y = _ball.InitialPosition.y;
            _distance = _ball.InitialPosition.y - _currentPosition.y;

            _tween = _ball.Rigidbody.DOMove(_targetPosition, GameManager.SlidingSpeed).SetEase(Ease.InSine);
            _tween.OnComplete(OnTweenCompleted);

            //GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            base.OnExit();

            //GameUpdateManager.RemoveUpdatable(this);

            _ball.Rigidbody.WakeUp();
        }

        public override void UpdateState()
        {
            _tween.PlayForward();

            //_currentPosition = _ball.InitialPosition + Vector3.up * (GameManager.SlidingSpeed / _distance) * Time.deltaTime;
            //_ball.Position = _currentPosition;

            //if (_ball.Position.y >= _ball.InitialPosition.y)
            //{
            //    _ball.Position = _ball.InitialPosition;
            //    OnComplete?.Invoke();
            //}
        } 
#endregion

        private void OnTweenCompleted()
        {
            OnComplete?.Invoke();
        }
    }
}