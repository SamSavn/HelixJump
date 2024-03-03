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
            _ball.Rigidbody.Sleep();
            _ball.transform.position = _ball.InitialPosition;
        }

        public override void OnExit()
        {
            base.OnExit();
            _ball.Rigidbody.WakeUp();
            _ball.SetActive(true);
        }

        public override void UpdateState()
        {
            
        }
    }
}