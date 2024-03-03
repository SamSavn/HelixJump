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
            _ball.Rigidbody.Sleep();
        }

        public override void OnExit()
        {
            base.OnExit();
            _ball.Rigidbody.WakeUp();
        }

        public override void UpdateState()
        {

        } 
#endregion
    }
}