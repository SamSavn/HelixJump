using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "LevelGenerationData", menuName = "Data/Level Generation")]
    public class LevelGenerationData : ScriptableObject
    {
        [SerializeField] private float _platformsDistance;
        [SerializeField] private float _initialSegmentAngleThreshold;
        [SerializeField][Range(0, 1f)] private float _platformsMinRandomization;
        [SerializeField][Range(0, 1f)] private float _platformsMaxRandomization;
        [SerializeField][Range(0, 1f)] private float _obstaclesMinRandomization;
        [SerializeField][Range(0, 1f)] private float _obstaclesMaxRandomization;

        public float PlatformsDistance => _platformsDistance;
        public float InitialSegmentAngleThreshold => _initialSegmentAngleThreshold;
        public float PlatformsMinRandomizationFactor => _platformsMinRandomization;
        public float PlatformsMaxRandomizationFactor => _platformsMaxRandomization;
        public float ObstaclesMinRandomizationFactor => _obstaclesMinRandomization;
        public float ObstaclesMaxRandomizationFactor => _obstaclesMaxRandomization;
    }
}