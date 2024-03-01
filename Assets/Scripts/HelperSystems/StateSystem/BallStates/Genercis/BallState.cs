using UnityEngine;
using LKS.GameElements;

namespace LKS.States
{
    public abstract class BallState : IState
    {
        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public abstract void UpdateState();
    }
}