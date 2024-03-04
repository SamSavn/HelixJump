using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "LevelGenerationData", menuName = "Data/Level Generation")]
    public class LevelGenerationData : ScriptableObject
    {
        [Header("Platforms")]
        [SerializeField][Min(0)] private float _platformsDistance;
        [SerializeField][Min(0)] private float _platformsDisableThreshold;
        [SerializeField][Min(0)] private float _initialSegmentAngleThreshold;
        [SerializeField][Range(1, 5)] private int _maxSegmentsInRow;
        [SerializeField][Range(0, 1f)] private float _platformsMinRandomization;
        [SerializeField][Range(0, 1f)] private float _platformsMaxRandomization;

        [Header("Obstacles")]
        [SerializeField][Range(0, 1f)] private float _obstaclesMinRandomization;
        [SerializeField][Range(0, 1f)] private float _obstaclesMaxRandomization;

        public float PlatformsDistance => _platformsDistance;
        public float PlatformsDisableThreshold => _platformsDisableThreshold;
        public float InitialSegmentAngleThreshold => _initialSegmentAngleThreshold;
        public float MaxSegmentsInRow => _maxSegmentsInRow;
        public float PlatformsMinRandomizationFactor => _platformsMinRandomization;
        public float PlatformsMaxRandomizationFactor => _platformsMaxRandomization;
        public float ObstaclesMinRandomizationFactor => _obstaclesMinRandomization;
        public float ObstaclesMaxRandomizationFactor => _obstaclesMaxRandomization;
    }
}