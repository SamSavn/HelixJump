using System;
using UnityEngine;

namespace LKS.AssetsManagement
{
    public partial class GameElementsFactory
    {
        private class GameElementSpawner : ISpawner
        {
#region Constants & Fields
            private GameObject _prefab;
            private GameObject _clone;
#endregion

#region Constructors
            public GameElementSpawner(string prefabAddress)
            {
                _prefab = AddressablesLoader.LoadSingle<GameObject>(prefabAddress);
            }
#endregion

#region Public Methods
            public bool TrySpawn<T>(out T obj) where T : class
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

            public void Dispose()
            {
                _prefab = null;
                _clone = null;
            } 
#endregion
        }
    }
}