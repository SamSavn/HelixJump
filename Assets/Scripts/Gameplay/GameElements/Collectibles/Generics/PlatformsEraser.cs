using LKS.Managers;
using UnityEngine;

namespace LKS.GameElements.Collectibles
{
    public class PlatformsEraser : Collectible
    {
        protected override void ToggleEffect(bool value)
        {
            CollectiblesManager.ToggleErasePlatforms(value);
        }
    }
}