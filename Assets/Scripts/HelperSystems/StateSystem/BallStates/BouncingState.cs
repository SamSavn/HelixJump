using LKS.GameElements;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class BouncingState : BallState
    {
#region Constants & Fields
        private float _bounceForce;
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
            UpdateState();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void UpdateState()
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _rigidbody.AddForce(new Vector3(0, _bounceForce / _rigidbody.mass, 0), ForceMode.VelocityChange);
        } 
#endregion
    }
}