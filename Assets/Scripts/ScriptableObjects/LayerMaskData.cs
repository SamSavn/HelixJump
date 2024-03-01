using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "LayerMaskData", menuName = "Data/Layer Mask")]
    public class LayerMaskData : ScriptableObject
    {
        [SerializeField] LayerMask _ballLayerMask;
        [SerializeField] LayerMask _platformMask;
        [SerializeField] LayerMask _platformSegmentMask;
        [SerializeField] LayerMask _obstacleMask;
        [SerializeField] LayerMask _fallZoneMask;

        public LayerMask BallLayerMask => _ballLayerMask;
        public LayerMask PlatformMask => _platformMask;
        public LayerMask PlatformSegmentMask => _platformSegmentMask;
        public LayerMask ObstacleMask => _obstacleMask;
        public LayerMask FallZoneMask => _fallZoneMask;
    }
}