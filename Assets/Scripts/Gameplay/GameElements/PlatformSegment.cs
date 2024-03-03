using LKS.Managers;
using System;
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
        public bool IsInitialSegment(float angleThreshold)
        {
            Vector3 toCamera = GameManager.GameCamera.Position - Position;
            toCamera.Normalize();

            float dot = Vector3.Dot(transform.forward, toCamera);
            return dot >= angleThreshold;
        }

        public void Initialize(Platform platform, bool activeForLevel)
        {
            _platform = platform;
            _activeForLevel = activeForLevel;

            if (_activeForLevel)
            {
                _platform.OnToggle += OnPlatformToggle;
                _platform.OnEnable += OnPlatformEnabled;
                _platform.OnDispose += Dispose;
            }

            SetActive(_activeForLevel);
        }

        public override void SetActive(bool active)
        {
            _mesh.SetActive(active);
            _collider.enabled = active;
            _renderer.enabled = active;
            base.SetActive(active);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_activeForLevel)
            {
                _platform.OnToggle -= OnPlatformToggle;
                _platform.OnEnable -= OnPlatformEnabled;
                _platform.OnDispose -= Dispose;
            }
        }
#endregion

#region Event Handlers
        private void OnPlatformToggle(bool value)
        {
            if (!_activeForLevel)
                return;

            SetActive(value);
        }

        private void OnPlatformEnabled(bool enabled)
        {
            _collider.enabled = enabled;
        }
#endregion
    }
}