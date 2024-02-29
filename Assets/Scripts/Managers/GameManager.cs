using LKS.GameElements;
using UnityEngine;

namespace LKS.Managers
{
    public static class GameManager
    {
        public static GameCamera GameCamera { get; private set; }
        public static Tower Tower { get; private set; }

        public static void SetGameCamera(GameCamera gameCamera)
        {
            GameCamera = gameCamera;
        }

        public static void SetTower(Tower tower)
        {
            Tower = tower;
        }
    }
}