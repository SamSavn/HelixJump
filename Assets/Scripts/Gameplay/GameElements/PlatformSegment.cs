using UnityEngine;

namespace LKS.Gameplay
{
    public class PlatformSegment : GameElement
    {
#region Constants & Fields
        private Platform _platform;
        private bool _activeForLevel;
        private int _id;
#endregion

#region Serialized Fields
        [SerializeField] GameObject _mesh;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Collider _collider;
#endregion

#region Public Methods
        public void Initialize(Platform platform, int id, bool activeForLevel)
        {
            _platform = platform;
            _platform.OnToggle += OnPlatformToggle;
            
            _id = id;

            _activeForLevel = activeForLevel;
            Toggle(_activeForLevel);
        }

        public void Toggle(bool value)
        {
            _mesh.SetActive(value);
        }
        #endregion

        #region Event Handlers
        private void OnPlatformToggle(bool value)
        {
            if (!_activeForLevel)
                return;

            Toggle(value);
        }

        private void OnPlatformRandomized(int id, bool value)
        {

        }
        #endregion
    }
}