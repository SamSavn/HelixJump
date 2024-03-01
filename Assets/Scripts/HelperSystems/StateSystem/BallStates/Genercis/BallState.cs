using UnityEngine;
using LKS.GameElements;

namespace LKS.States
{
    public abstract class BallState : IState
    {
        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }

        public abstract void UpdateState();
    }
}