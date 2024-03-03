using LKS.GameElements;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class DeadState : BallState
    {
#region Constructors
        public DeadState(Ball ball) : base(ball)
        {
        }
#endregion

#region Public Methods
        public override void OnEnter()
        {
            base.OnEnter();
            _ball.Rigidbody.velocity = Vector3.zero;
            _ball.Rigidbody.isKinematic = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            _ball.Rigidbody.isKinematic = false;
        }

        public override void UpdateState()
        {

        } 
#endregion
    }
}