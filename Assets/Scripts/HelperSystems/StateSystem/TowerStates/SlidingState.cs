using LKS.Extentions;
using LKS.GameElements;
using LKS.GameUpdate;
using LKS.Iterations;
using LKS.Managers;
using UnityEngine;

namespace LKS.States.TowerStates
{
    public class SlidingState : TowerState
    {
        private Vector3 _startingPosition;
        private Vector3 _currentPosition;

        public SlidingState(Tower tower) : base(tower)
        {
            _startingPosition = _tower.Position;
            _currentPosition = _startingPosition;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            GameUpdateManager.RemoveUpdatable(this);
            base.OnExit();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            _currentPosition.y = _startingPosition.y + GameManager.Ball.Position.y;
            _tower.Position = _currentPosition;
        }
    }
}