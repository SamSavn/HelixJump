using LKS.GameElements;
using LKS.Managers;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class BouncingState : BallState
    {
#region Constants & Fields
        private const float MAX_JUMP_HEIGHT = 2f;
        private float _bounceForce;
        private bool _bounced;
#endregion

#region Properties
        private Rigidbody _rigidbody => _ball.Rigidbody;
#endregion

#region Constructors
        public BouncingState(Ball ball, float bounceForce) : base(ball)
        {
            _bounceForce = bounceForce;
        }
#endregion

#region Public Methods
        public override void OnEnter()
        {
            base.OnEnter();

            _bounced = false;
            UpdateState();

            GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            base.OnExit();
            GameUpdateManager.RemoveUpdatable(this);
        }

        public override void UpdateState()
        {
            if (!_bounced)
            {
                _bounced = true;
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
                _rigidbody.AddForce(new Vector3(0, _bounceForce / _rigidbody.mass, 0), ForceMode.VelocityChange); 
            }
            else if (_ball.Position.y > MAX_JUMP_HEIGHT)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            }
        } 
#endregion
    }
}