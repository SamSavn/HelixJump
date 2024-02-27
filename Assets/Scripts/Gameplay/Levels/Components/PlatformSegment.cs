using UnityEngine;

namespace LKS.Gameplay
{
    public class PlatformSegment : MonoBehaviour
    {
        [SerializeField] GameObject _mesh;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Collider _collider;

        public void Toggle(bool value)
        {
            _mesh.SetActive(value);
        }
    }
}