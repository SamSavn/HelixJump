using LKS.Data;
using LKS.GameElements;
using LKS.Managers;
using UnityEngine;

namespace LKS.States.PlatformStates
{
    public class IdleState : PlatformState
    {
        float _threshold;

        public IdleState(Platform platform, LevelGenerationData levelGenerationData) : base(platform)
        {
            _threshold = levelGenerationData.PlatformsDistance - levelGenerationData.PlatformsDisableThreshold;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _platform.SetActive(GameManager.CanActivatePlatform(_platform));
            _platform.SetEnabled(_platform.LocalPosition.y < _threshold);
        }
    }
}