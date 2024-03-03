using LKS.GameElements;
using UnityEngine;

namespace LKS.States.BallStates
{
    public class WaitingState : BallState
    {
        public WaitingState(Ball ball) : base(ball)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _ball.SetActive(false);
            _ball.Rigidbody.velocity = Vector3.zero;
            _ball.Rigidbody.isKinematic = true;
            _ball.transform.position = _ball.InitialPosition;
        }

        public override void OnExit()
        {
            base.OnExit();
            _ball.Rigidbody.isKinematic = false;
            _ball.SetActive(true);
        }

        public override void UpdateState()
        {
            
        }
    }
}