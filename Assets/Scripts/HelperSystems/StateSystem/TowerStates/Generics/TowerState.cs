using LKS.GameElements;
using UnityEngine;

namespace LKS.States.TowerStates
{
    public class TowerState : IState
    {
        protected Tower _tower;

        public TowerState(Tower tower)
        {
            _tower = tower;
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