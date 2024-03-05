using System;
using UnityEngine;

namespace LKS.GameElements
{
    public class FallZone : GameElement
    {
        public event Action OnBallEnter;
        [SerializeField] private Collider _collider;

        private void Awake()
        {
            _collider ??= GetComponent<Collider>();
            SetActive(true);
        }

        public void OnHit()
        {
            SetActive(false);
            OnBallEnter?.Invoke();
        }

        public override void SetActive(bool active)
        {
            _collider.enabled = active;
        }
    }
}