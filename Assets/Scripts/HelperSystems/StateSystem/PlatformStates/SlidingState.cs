using LKS.GameElements;
using LKS.Managers;
using System;
using UnityEngine;

namespace LKS.States.PlatformStates
{
    public class SlidingState : PlatformState
    {
        private Action OnComplete;
        private float _slidingDistance;

        public SlidingState(Platform platform, float distance, Action onComplete) : base(platform)
        {
            OnComplete = onComplete;
            _slidingDistance = distance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameUpdateManager.AddUpdatable(this);
        }

        public override void OnExit()
        {
            base.OnExit();
            GameUpdateManager.RemoveUpdatable(this);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector3 pos = _platform.LocalPosition;
            pos.y += _slidingDistance;
            _platform.LocalPosition = pos;

            OnComplete?.Invoke();
        }
    }
}