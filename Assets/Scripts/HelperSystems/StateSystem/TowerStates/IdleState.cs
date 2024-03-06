using LKS.GameElements;
using UnityEngine;

namespace LKS.States.TowerStates
{
    public class IdleState : TowerState
    {
        public IdleState(Tower tower) : base(tower)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _tower.CheckForLevelUpdate();
        }
    }
}