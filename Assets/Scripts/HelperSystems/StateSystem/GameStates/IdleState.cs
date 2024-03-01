using LKS.GameElements;
using UnityEngine;

namespace LKS.States
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

            _ball.Rigidbody.velocity = Vector3.zero;
            _ball.Rigidbody.isKinematic = true;
            _ball.transform.position = _ball.InitialPosition;
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