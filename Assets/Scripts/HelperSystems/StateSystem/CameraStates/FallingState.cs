using LKS.GameElements;
using LKS.Managers;
using UnityEngine;

namespace LKS.States.CameraStates
{
    public class FallingState : CameraState
    {
        private Vector3 _startingPosition;
        private Vector3 _currentPosition;
        private float _distanceFromBall;

        public FallingState(GameCamera camera) : base(camera)
        {
            _startingPosition = _camera.Position;
            _currentPosition = _startingPosition;
            _distanceFromBall = _startingPosition.y - GameManager.Ball.Position.y;
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

        public override void UpdateState()
        {
            base.UpdateState();

            _currentPosition.y = _startingPosition.y + GameManager.Ball.Position.y;
            _camera.Position = _currentPosition;
        }
    }
}