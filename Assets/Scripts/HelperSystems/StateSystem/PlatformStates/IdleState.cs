using LKS.Data;
using LKS.GameElements;
using LKS.Managers;
using UnityEngine;

namespace LKS.States.PlatformStates
{
    public class IdleState : PlatformState
    {
        public IdleState(Platform platform) : base(platform)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _platform.SetActive(GameManager.CanActivatePlatform(_platform));
            _platform.SetEnabled(_platform.LocalPosition.y < 1);
        }
    }
}