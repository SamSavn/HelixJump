using LKS.GameElements;
using LKS.Managers;
using System;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class FallingState : BallState
    {
        private Action OnComplete;
        private Vector3 _position;
        private float _speed;
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

            _ball.Rigidbody.isKinematic = true;

            _speed = Mathf.Abs(_ball.Rigidbody.velocity.y) / _ball.Rigidbody.mass;
            _position = _ball.Position;
            _distance = _ball.InitialPosition.y - _position.y;

            GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            base.OnExit();

            GameUpdateManager.RemoveUpdatable(this);

            _ball.Rigidbody.velocity = Vector3.zero;
            _ball.Rigidbody.isKinematic = false;
        }

        public override void UpdateState()
        {
            _position.y += _distance / GameManager.SlidingSpeed * Time.deltaTime;
            _ball.Position = _position;

            if (_ball.Position.y >= _ball.InitialPosition.y)
            {
                _ball.Position = _ball.InitialPosition;
                OnComplete?.Invoke();
            }
        } 
#endregion
    }
}