using LKS.GameElements;
using LKS.Iterations;
using UnityEngine;

namespace LKS.States.TowerStates
{
    public class SlidingState : TowerState
    {
        private Iterator _iterator;

        public SlidingState(Tower tower, Iterator iterator) : base(tower)
        {
            _iterator = iterator;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _iterator.Iterate();
        }
    }
}