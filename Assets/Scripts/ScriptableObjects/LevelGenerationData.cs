using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "LevelGenerationData", menuName = "Data/Level Generation")]
    public class LevelGenerationData : ScriptableObject
    {
        [SerializeField] [Min(1)] private int _additionalPlatformsOffset;
        [SerializeField] private float _platformsDistance;
        [SerializeField] private float _initialSegmentAngleThreshold;
        [SerializeField] [Range(0, 1f)] private float _platformsMinRandomization;
        [SerializeField] [Range(0, 1f)] private float _platformsMaxRandomization;

        public int AdditionalPlatformsOffset => _additionalPlatformsOffset;
        public float PlatformsDistance => _platformsDistance;
        public float PlatformsMinRandomization => _platformsMinRandomization;
        public float PlatformsMaxRandomizationFactor => _platformsMaxRandomization;
        public float InitialSegmentAngleThreshold => _initialSegmentAngleThreshold;
    }
}