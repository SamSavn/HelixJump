using LKS.GameElements;
using LKS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LKS.Pooling
{
    public class ObjectPool<T> : IDisposable where T : GameElement
    {
#region Constants & Fields
        private readonly Queue<T> _pool = new Queue<T>();
#endregion

#region Constructors
        public ObjectPool()
        {
            _pool = new Queue<T>();
        }

        public ObjectPool(IEnumerable<T> objects) : this()
        {
            int count = objects.Count();
            for (int i = 0; i < count; i++)
            {
                if (objects.ElementAt(i) is not T poolObj)
                    continue;

                AddToPool(poolObj);
            }
        }
#endregion

#region Public Methods
        public T SpawnFromPool()
        {
            if (_pool.Count == 0)
            {
                Debug.LogWarning($"Object pool of type {typeof(T).Name} is empty.");
                return null;
            }

            T poolObj = _pool.Dequeue();
            poolObj.SetActive(true);
            return poolObj;
        }

        public void AddToPool(T poolObj)
        {
            if (poolObj == null)
                return;

            poolObj.SetActive(false);
            _pool.Enqueue(poolObj);
        }

        public void Dispose()
        {
            GameElement ge;
            while (_pool.Count > 0)
            {
                ge = _pool.Dequeue();

                if (ge != null)
                {
                    GameObject.Destroy(ge);
                }
            }

            _pool.Clear();
        } 
#endregion
    }
}