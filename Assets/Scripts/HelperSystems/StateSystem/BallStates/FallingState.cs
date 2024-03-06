using LKS.GameElements;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class FallingState : BallState
    {
#region Constructors
        public FallingState(Ball ball) : base(ball)
        {
        }
#endregion

#region Public Methods
        public override void OnEnter()
        {
            base.OnEnter();

            //_ball.Rigidbody.velocity = Vector3.zero;
            //_ball.Rigidbody.useGravity = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            //_ball.Rigidbody.useGravity = true;
        }

        public override void UpdateState()
        {
            
        } 
#endregion
    }
}