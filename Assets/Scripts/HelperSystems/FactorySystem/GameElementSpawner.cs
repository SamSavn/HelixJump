using LKS.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.AssetsManagement
{
    public partial class GameElementsFactory
    {
        private class GameElementSpawner : ISpawner
        {
#region Constants & Fields
            private List<GameObject> _allPrefabs;
            private GameObject _prefab;
            private GameObject _clone;
#endregion

#region Constructors
            public GameElementSpawner(string prefabAddress, bool loadAll = false)
            {
                if (!loadAll)
                {
                    _prefab = AddressablesLoader.LoadSingle<GameObject>(prefabAddress);
                }
                else
                {
                    _allPrefabs = AddressablesLoader.LoadAll<GameObject>(prefabAddress);
                }
            }
#endregion

#region Public Methods
            public bool TrySpawn<T>(out T obj) where T : Component
            {
                _clone = GameObject.Instantiate(_prefab);

                if (_clone == null)
                {
                    Debug.LogError($"Unable to create object of type {typeof(T).Name}");
                    obj = null;
                    return false;
                }

                obj = _clone.GetComponent<T>();
                return obj != null;
            }

            public bool TrySpawnAll<T>(out List<T> list) where T : Component
            {
                List<T> result = null;

                for (int i = 0; i < _allPrefabs.Count; i++)
                {
                    _prefab = _allPrefabs[i];

                    if(TrySpawn(out T obj))
                    {
                        result ??= new List<T>();
                        result.Add(obj);
                    }
                }

                list = result;
                return result != null;
            }

            public void Dispose()
            {
                _prefab = null;
                _clone = null;
            } 
#endregion
        }
    }
}