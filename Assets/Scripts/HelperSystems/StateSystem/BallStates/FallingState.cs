using LKS.GameElements;

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
            _ball.Rigidbody.useGravity = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            _ball.Rigidbody.useGravity = true;
        }

        public override void UpdateState()
        {
            
        } 
#endregion
    }
}