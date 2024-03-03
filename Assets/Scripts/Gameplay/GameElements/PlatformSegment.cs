using LKS.Managers;
using System;
using UnityEngine;

namespace LKS.GameElements
{
    public class PlatformSegment : GameElement
    {
#region Constants & Fields
        private Platform _platform;
        private PlatformSegmentComponent _activeComponent;
        private bool _activeForLevel;
#endregion

#region Serialized Fields
        [SerializeField] private PlatformSegmentComponent _segment;
        [SerializeField] private PlatformSegmentComponent _obstacle;
#endregion

#region Unity Methods
        private void Awake()
        {
            _segment.SetActive(false);
            _obstacle.SetActive(false);
        } 
#endregion

#region Public Methods
        public bool IsInitialSegment(float angleThreshold)
        {
            Vector3 toCamera = GameManager.GameCamera.Position - Position;
            toCamera.Normalize();

            float dot = Vector3.Dot(transform.forward, toCamera);
            return dot >= angleThreshold;
        }

        public void Initialize(Platform platform, bool activeForLevel, bool isObstacle)
        {
            _segment.SetActive(false);
            _obstacle.SetActive(false);

            _platform = platform;
            _activeForLevel = activeForLevel;
            _activeComponent = isObstacle ? _obstacle : _segment;
            _activeComponent.SetActive(true);

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
            _activeComponent.SetActive(active);
            base.SetActive(active);
        }

        public override void Dispose()
        {
            base.Dispose();

            _segment.SetActive(false);
            _obstacle.SetActive(false);

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
            _activeComponent.ToggleCollider(enabled);
        }
#endregion
    }
}