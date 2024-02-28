using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "LevelGenerationData", menuName = "Data/Level Generation")]
    public class LevelGenerationData : ScriptableObject
    {
        [SerializeField] private float _platformsDistance;

        public float PlatformsDistance => _platformsDistance;
    }
}