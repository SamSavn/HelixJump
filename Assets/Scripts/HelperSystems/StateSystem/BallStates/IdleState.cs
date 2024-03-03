using LKS.GameElements;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class IdleState : BallState
    {
#region Constructors
        public IdleState(Ball ball) : base(ball)
        {
        }
#endregion

#region Public Methods
        public override void OnEnter()
        {
            base.OnEnter();
            _ball.Rigidbody.WakeUp();
        }

        public override void OnExit()
        {
            base.OnExit();            
        }

        public override void UpdateState()
        {

        } 
#endregion
    }
}