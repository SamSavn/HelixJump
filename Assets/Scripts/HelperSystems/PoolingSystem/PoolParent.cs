using UnityEngine;

namespace LKS.Pooling
{
    public class PoolParent : MonoBehaviour
    {
        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Add<T>(T child) where T : Component
        {
            child.transform.SetParent(transform);
        }
    }
}