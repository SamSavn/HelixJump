using LKS.GameElements;
using UnityEngine;

namespace LKS.States
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
        }

        public override void OnExit()
        {
            base.OnExit();
            _ball.Rigidbody.velocity = Vector3.zero;
        }

        public override void UpdateState()
        {

        } 
#endregion
    }
}