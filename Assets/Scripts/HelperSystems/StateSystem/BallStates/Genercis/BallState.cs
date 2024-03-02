using LKS.GameElements;

namespace LKS.States.BallStates
{
    public abstract class BallState : IState
    {
#region Constants & Fields
        protected Ball _ball;
#endregion

#region Constructors
        public BallState(Ball ball)
        {
            _ball = ball;
        }
#endregion

#region Public Methods
        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {
        }

        public abstract void UpdateState(); 
#endregion
    }
}