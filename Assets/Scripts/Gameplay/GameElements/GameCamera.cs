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

        public Vector3 CameraPosition => _camera.transform.position;

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

        protected override void AddListeners()
        {
            base.AddListeners();
            GameManager.BallStateChanged += OnBallStateChanged;
        }

        protected override void RemoveListeners()
        {
            GameManager.BallStateChanged -= OnBallStateChanged;
            base.RemoveListeners();
        }

        private void OnBallStateChanged(States.BallStates.BallState state)
        {
            if (state.GetType() == typeof(States.BallStates.MovingState))
            {
                Move();
            }
        }
    }
}