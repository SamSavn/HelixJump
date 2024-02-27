using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "LevelGenerationData", menuName = "Data/Level Generation")]
    public class LevelGenerationData : ScriptableObject
    {
        [SerializeField] private GameObject _platformPrefab;

        public GameObject PlatformPrefab => _platformPrefab;
    }
}