using LKS.GameElements;
using LKS.GameUpdate;
using LKS.Managers;

namespace LKS.States.BallStates
{
    public abstract class BallState : IState, IUpdatable
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
            GameManager.OnBallStateChanged(this);
        }

        public virtual void OnExit()
        {

        }

        public void CustomUpdate()
        {
            UpdateState();
        }

        public abstract void UpdateState(); 
#endregion
    }
}