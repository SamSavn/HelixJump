using LKS.AssetsManagement;
using LKS.GameElements;
using UnityEngine;

namespace LKS.Managers
{
    public static class AssetsManager
    {
        private static readonly GameElementsFactory _gameElementFactory;

        static AssetsManager()
        {
            _gameElementFactory = new GameElementsFactory();
        }

        public static Platform GetPlatform()
        {
            return _gameElementFactory.CreatePlatform();
        }
    }
}