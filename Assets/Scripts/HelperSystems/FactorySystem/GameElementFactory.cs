using System.Collections.Generic;
using LKS.GameElements;
using LKS.GameElements.Collectibles;

namespace LKS.AssetsManagement
{
    public partial class GameElementsFactory
    {
#region Constants & Fields
        private const string PLATFORM_PREFAB_ADDRESS = "Platform";
        private const string COLLECTIBLES_ADDRESS = "Collectibles";

        private readonly Dictionary<string, ISpawner> _spawners;
#endregion

#region Constructors
        public GameElementsFactory()
        {
            _spawners = new Dictionary<string, ISpawner>();
        }
#endregion

#region Private Methods
        private ISpawner GetSpawner(string address, bool loadAll = false)
        {
            if (_spawners.TryGetValue(address, out ISpawner spawnable))
            {
                return spawnable;
            }

            _spawners[address] = new GameElementSpawner(address, loadAll);
            return _spawners[address];
        }
#endregion

#region Public Methods
        public T Create<T>(string address) where T : GameElement
        {
            if (GetSpawner(address).TrySpawn(out T obj))
            {
                obj.gameObject.name = $"{typeof(T).Name} {obj.Id}";
                return obj;
            }

            return null;
        }

        public List<T> CreateAll<T>(string address) where T : GameElement
        {
            if (GetSpawner(address, loadAll: true).TrySpawnAll(out List<T> objs))
            {
                foreach (T obj in objs)
                {
                    obj.gameObject.name = $"{typeof(T).Name} {obj.Id}";
                }

                return objs;
            }

            return null;
        }

        public Platform CreatePlatform()
        {
            return Create<Platform>(PLATFORM_PREFAB_ADDRESS);
        }

        public PlatformsEraser CreatePlatformsEraser()
        {
            return Create<PlatformsEraser>(COLLECTIBLES_ADDRESS);
        }

        public List<Collectible> CreateCollectibles()
        {
            return CreateAll<Collectible>(COLLECTIBLES_ADDRESS);
        }
#endregion
    }
}