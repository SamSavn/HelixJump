using System.Collections.Generic;
using LKS.GameElements;

namespace LKS.AssetsManagement
{
    public partial class GameElementsFactory
    {
#region Constants & Fields
        private const string PLATFORM_PREFAB_ADDRESS = "Platform";
        private const string OBSTACLE_PREFAB_ADDRESS = "Obstacle";

        private readonly Dictionary<string, ISpawner> _spawners;
#endregion

#region Constructors
        public GameElementsFactory()
        {
            _spawners = new Dictionary<string, ISpawner>();
        }
#endregion

#region Private Methods
        private ISpawner Create(string address)
        {
            if (_spawners.TryGetValue(address, out ISpawner spawnable))
            {
                return spawnable;
            }

            _spawners[address] = new GameElementSpawner(address);
            return _spawners[address];
        }
#endregion

#region Public Methods
        public T Create<T>(string address) where T : GameElement
        {
            if (Create(address).TrySpawn(out T obj))
            {
                obj.gameObject.name = $"{typeof(T).Name} {obj.Id}";
                return obj;
            }

            return null;
        }

        public Platform CreatePlatform()
        {
            return Create<Platform>(PLATFORM_PREFAB_ADDRESS);
        } 
#endregion
    }
}