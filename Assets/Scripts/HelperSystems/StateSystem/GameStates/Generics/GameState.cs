using UnityEngine;

namespace LKS.States.GameStates
{
    public abstract class GameState : IState
    {

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void UpdateState()
        {
            
        }

        public void CustomUpdate()
        {
            UpdateState();
        }
    }
}