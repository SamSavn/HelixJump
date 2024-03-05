using LKS.GameElements;
using LKS.Managers;
using UnityEngine;

namespace LKS.States.CameraStates
{
    public class MovingState : CameraState
    {
        private Tower _tower;
        private Ball _ball;

        public MovingState(GameCamera camera) : base(camera)
        {
            _ball = GameManager.Ball;
            _tower = GameManager.Tower;
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

            if (_ball == null)
                return;
            
            Vector3 towerToBall = _ball.Position - _tower.Position;
            Quaternion lookRotation = Quaternion.LookRotation(-towerToBall);
            _camera.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}