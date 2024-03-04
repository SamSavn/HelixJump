using UnityEngine;

namespace LKS.Utils
{
    public abstract class PersistentMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}