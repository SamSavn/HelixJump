using UnityEngine;

namespace LKS.Data
{
    [CreateAssetMenu(fileName = "GameElementsPoolData", menuName = "Data/Game Elements Pool Data")]
    public class GameElementsPoolData : ScriptableObject
    {
        [SerializeField] private int _platforms;

        [Header("Collectibles")]
        [SerializeField] private int _platformErasers;

        public int Platforms => _platforms;
        public int PlatformErasers => _platformErasers;
    }
}