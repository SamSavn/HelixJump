using LKS.Managers;
using LKS.States;
using LKS.States.CameraStates;
using UnityEngine;

using BallState = LKS.States.BallStates.BallState;
using BallFallingState = LKS.States.BallStates.FallingState;

namespace LKS.GameElements
{
    public class GameCamera : GameElement
    {
#region Constants & Fields
        private StateMachine<CameraState> _stateMachine;
#endregion

#region Serialized Fields
        [SerializeField] private Camera _camera;
#endregion

#region Properties
        public Vector3 CameraPosition => _camera.transform.position;
#endregion

#region Unity Methods
        private void Awake()
        {
            GameManager.SetGameCamera(this);
            _stateMachine = new StateMachine<CameraState>(new IdleState(this));
        }
#endregion

#region Public Methods
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

        public void Fall()
        {
            if (!_stateMachine.HasState<FallingState>())
            {
                _stateMachine.ChangeState(new FallingState(this));
            }
        }
#endregion

#region Private Methods
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
#endregion

#region Event Handlers
        private void OnBallStateChanged(BallState state)
        {
            if (state.GetType() == typeof(BallFallingState))
            {
                Fall();
            }
            else
            {
                Move();
            }
        } 
#endregion
    }
}