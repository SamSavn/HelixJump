using LKS.Utils;
using UnityEngine;

namespace LKS.Pooling
{
    public class PoolParent : PersistentMonoBehaviour
    {
        public void Add<T>(T child) where T : Component
        {
            child.transform.SetParent(transform);
        }
    }
}