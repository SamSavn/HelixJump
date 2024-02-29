using LKS.Managers;
using UnityEngine;

namespace LKS.GameElements
{
    public class PlatformSegment : GameElement
    {
#region Constants & Fields
        private Platform _platform;
        private bool _activeForLevel;
#endregion

#region Serialized Fields
        [SerializeField] GameObject _mesh;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Collider _collider;
#endregion

#region Public Methods
        public bool IsInitialSegment()
        {
            float angleThreshold = 0.6f;
            Vector3 toCamera = GameManager.GameCamera.Position - Position;
            toCamera.Normalize();

            float dot = Vector3.Dot(transform.forward, toCamera);
            return dot >= angleThreshold;
        }

        public void Initialize(Platform platform, bool activeForLevel)
        {
            _platform = platform;
            _platform.OnToggle += OnPlatformToggle;

            _activeForLevel = activeForLevel;
            SetActive(_activeForLevel);
        }

        public override void SetActive(bool active)
        {
            _mesh.SetActive(active);
            _collider.enabled = active;
            _renderer.enabled = active;
            base.SetActive(active);
        }
#endregion

#region Event Handlers
        private void OnPlatformToggle(bool value)
        {
            if (!_activeForLevel)
                return;

            SetActive(value);
        }
#endregion
    }
}