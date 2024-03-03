using UnityEngine;

namespace LKS.GameElements
{
    public class PlatformSegmentComponent : GameElement
    {
        #region Serialized Fields
        [SerializeField] GameObject _mesh;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Collider _collider;
        #endregion

        public override void SetActive(bool active)
        {
            _mesh.SetActive(active);
            _collider.enabled = active;
            _renderer.enabled = active;
            base.SetActive(active);
        }

        public void ToggleCollider(bool value)
        {
            _collider.enabled = value;
        }
    }
}