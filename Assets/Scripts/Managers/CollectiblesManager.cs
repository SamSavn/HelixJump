using LKS.GameElements;
using LKS.GameElements.Collectibles;
using LKS.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.Managers
{
    public static class CollectiblesManager
    {
        private const string COLLECTIBLES_ADDRESS = "Collectibles";
        private static readonly List<Collectible> _availableCollectibles;

        static CollectiblesManager()
        {
            _availableCollectibles = AddressablesLoader.LoadAll<Collectible>(COLLECTIBLES_ADDRESS);
        }

        public static void ToggleErasePlatforms(bool value)
        {
            GameManager.Tower.TogglePlatforms();
        }
    }
}