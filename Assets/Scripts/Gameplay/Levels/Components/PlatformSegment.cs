using UnityEngine;

namespace LKS.Gameplay
{
    public class PlatformSegment : MonoBehaviour
    {
#region Serialized Fields
        [SerializeField] GameObject _mesh;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Collider _collider;
#endregion

#region Public Methods
        public void Toggle(bool value)
        {
            _mesh.SetActive(value);
        } 
#endregion
    }
}