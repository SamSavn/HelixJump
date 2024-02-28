using LKS.Gameplay;
using UnityEngine;

namespace LKS.Managers
{
    public static class GameManager
    {
        public static Tower Tower { get; private set; }

        public static void SetTower(Tower tower)
        {
            Tower = tower;
        }
    }
}