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
            _distance = _ball.InitialPosition.y - _currentPosition.y;

            GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            base.OnExit();

            GameUpdateManager.RemoveUpdatable(this);

            _ball.Rigidbody.WakeUp();
        }

        public override void UpdateState()
        {
            _currentPosition = _ball.Position;
            _currentPosition.y += GameManager.SlidingSpeed / _distance * Time.deltaTime;
            _ball.Position = _ball.InitialPosition;

            if (_ball.Position.y >= _ball.InitialPosition.y)
            {
                _ball.Position = _ball.InitialPosition;
                OnComplete?.Invoke();
            }
        } 
#endregion
    }
}