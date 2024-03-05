using LKS.GameElements;
using LKS.Inputs;
using LKS.Managers;
using LKS.Utils;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class MovingState : BallState
    {
        private SwipeInfo _swipeInfo;

        private Vector3 _currentPosition;
        private Vector3 _newPosition;

        private float _angle;
        private int _rotationMultiplier;

        public MovingState(Ball ball, SwipeInfo info) : base(ball)
        {
            UpdateInfo(info);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            GameUpdateManager.RemoveUpdatable(this);
            base.OnExit();
        }

        public void UpdateInfo(SwipeInfo swipeInfo)
        {
            _swipeInfo = swipeInfo;
        }

        public override void UpdateState()
        {
            _angle = _swipeInfo.Distance.x / Screen.width * 360f;
            _rotationMultiplier = (_swipeInfo.Direction.x == Direction.Left) ? 1 : -1;

            _currentPosition = _ball.Position;
            _newPosition = Quaternion.Euler(0f, _rotationMultiplier * _angle * Time.deltaTime, 0f) * _currentPosition;
            _newPosition = _newPosition.normalized * _currentPosition.magnitude;

            _ball.Position = _newPosition;
        }
    }
}