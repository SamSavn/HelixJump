using LKS.AssetsManagement;
using LKS.Data;
using LKS.GameElements;
using LKS.Helpers;
using LKS.Pooling;
using UnityEngine;

namespace LKS.Managers
{
    public static class PoolingManager
    {
#region Constants & Fields
        private const string GE_POOL_DATA_ADDRESS = "GameElementsPoolData";

        private static readonly GameElementsPoolData _gameElementsData;
        private static readonly GameElementsFactory _gameElementFactory;
        private static readonly ObjectPool<GameElement> _gameElementsPool;

        private static PoolParent _poolParent;
#endregion

#region Constructors
        static PoolingManager()
        {
            _poolParent = new GameObject("Pool").AddComponent<PoolParent>();
            _poolParent.Initialize();

            _gameElementsData = AddressablesLoader.LoadSingle<GameElementsPoolData>(GE_POOL_DATA_ADDRESS);
            _gameElementsPool = new ObjectPool<GameElement>();
            _gameElementFactory = new GameElementsFactory();

            PoolGameElements();
        }
#endregion

#region Public Methods
        public static T GetFromPool<T>() where T : GameElement
        {
            GameElement ge = _gameElementsPool.SpawnFromPool();

            if (ge is T result)
            {
                return result;
            }

            return null;
        }

        public static void AddToPool(GameElement gameElement)
        {
            gameElement.SetActive(false);
            gameElement.Dispose();
            _poolParent.Add(gameElement);
            _gameElementsPool.AddToPool(gameElement);
        }
#endregion

#region Private Methods
        private static void PoolGameElements()
        {
            for (int i = 0; i < _gameElementsData.Platforms; i++)
            {
                AddToPool(_gameElementFactory.CreatePlatform());
            }
        }
#endregion
    }
}