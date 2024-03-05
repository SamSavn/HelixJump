using LKS.GameUpdate;
using LKS.Managers;
using LKS.States;
using LKS.States.CameraStates;
using UnityEngine;

namespace LKS.GameElements
{
    public class GameCamera : GameElement
    {
        private StateMachine<CameraState> _stateMachine;
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            _stateMachine = new StateMachine<CameraState>(new IdleState(this));
            GameManager.SetGameCamera(this);
        }

        public void Idle()
        {
            _stateMachine.ChangeState(new IdleState(this));
        }

        public void Move()
        {
            if (!_stateMachine.HasState<MovingState>())
            {
                _stateMachine.ChangeState(new MovingState(this));
            }
            else
            {
                _stateMachine.UpdateStates();
            }
        }
    }
}