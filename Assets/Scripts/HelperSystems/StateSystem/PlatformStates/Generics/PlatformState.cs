using LKS.GameElements;
using UnityEngine;

namespace LKS.States.PlatformStates
{
    public abstract class PlatformState : IState
    {
        protected Platform _platform;

        public PlatformState(Platform platform)
        {
            _platform = platform;
        }

        public void CustomUpdate()
        {
            UpdateState();
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void UpdateState()
        {
            
        }
    }
}