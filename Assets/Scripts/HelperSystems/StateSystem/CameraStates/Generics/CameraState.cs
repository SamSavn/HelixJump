using LKS.GameElements;
using UnityEngine;

namespace LKS.States.CameraStates
{
    public abstract class CameraState : IState
    {
        protected GameCamera _camera;

        public CameraState(GameCamera camera)
        {
            _camera = camera;
        }

        public virtual void CustomUpdate()
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