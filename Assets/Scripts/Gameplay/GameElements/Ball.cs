using LKS.Managers;
using LKS.States;
using LKS.States.BallStates;
using UnityEngine;

namespace LKS.GameElements
{
    public class Ball : GameElement
    {
#region Serialized Fields
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _jumpForce;
#endregion

#region Fields
        private StateMachine<BallState> _stateMachine;
#endregion

#region Properties
        public Rigidbody Rigidbody => _rigidbody;
        public Vector3 InitialPosition {  get; private set; }
#endregion

#region Unity Methods
        private void Awake()
        {            
            InitialPosition = Position;
            _stateMachine = new StateMachine<BallState>(new WaitingState(this));

            GameManager.SetBall(this);
        }
#endregion

#region Public Methods
        public void StartGame()
        {
            _stateMachine?.ChangeState(new IdleState(this));
        }
#endregion

#region Private Methods
        private void OnSegmentHit(Collision collision)
        {
            if (Vector3.Dot(collision.GetContact(0).normal, Vector3.up) > 0.8f)
            {
                Bounce();
            }
        }

        private void OnObstacleHit(Collision _)
        {
            Die();
        }
#endregion

#region States Methods
        private void Bounce()
        {
            if (_stateMachine.IsInState<BouncingState>())
            {
                _stateMachine.RestartState();
            }
            else
            {
                _stateMachine.ChangeState(new BouncingState(this, _jumpForce));
            }
        }

        private void Fall()
        {
            if (_stateMachine.IsInState<FallingState>())
            {
                _stateMachine.RestartState();
            }
            else
            {
                _stateMachine.ChangeState(new FallingState(this));
            }
        }

        private void Die()
        {
            _stateMachine.ChangeState(new DeadState(this));
        } 

        private void ResetState()
        {
            _stateMachine.Reset();
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            _collider.enabled = active;

            if(active)
            {
                _rigidbody.WakeUp();
            }
            else
            {
                _rigidbody.Sleep();
            }
        }
#endregion

#region Collision Detection
        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            if (LayerMaskManager.IsPlatformSegment(collision.gameObject))
            {
                OnSegmentHit(collision);
            }
            else if (LayerMaskManager.IsObstacle(collision.gameObject))
            {
                OnObstacleHit(collision);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LayerMaskManager.IsFallZone(other.gameObject))
            {
                Fall();
            }
        }
#endregion
    }
}