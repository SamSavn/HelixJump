using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "LevelGenerationData", menuName = "Data/Level Generation")]
    public class LevelGenerationData : ScriptableObject
    {
        [SerializeField] private GameObject _platformPrefab;
        [SerializeField] private float _platformsDistance;

        public GameObject PlatformPrefab => _platformPrefab;
        public float PlatformsDistance => _platformsDistance;
    }
}