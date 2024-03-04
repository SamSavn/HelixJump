using System.Collections;
using UnityEngine;

namespace LKS.GameElements.Collectibles
{
    public abstract class Collectible : GameElement
    {
        private WaitForSeconds _waitForDeactivating;

        [SerializeField][Range(10f, 30f)] private float _duration = 10f;

        protected virtual void Awake()
        {
            _waitForDeactivating = new WaitForSeconds(_duration);
        }

        public virtual void Collect()
        {
            SetActive(false);
        }

        public virtual void Activate()
        {
            ToggleEffect(true);
        }

        public virtual void Deactivate(bool immediate = false)
        {
            StartCoroutine(DeactivateCo(immediate));
        }

        protected abstract void ToggleEffect(bool value);

        private IEnumerator DeactivateCo(bool immediate)
        {
            if (!immediate)
            {
                yield return _waitForDeactivating; 
            }

            ToggleEffect(false);
        }
    }
}