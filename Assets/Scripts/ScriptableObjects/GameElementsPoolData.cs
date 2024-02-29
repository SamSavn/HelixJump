using UnityEngine;

namespace LKS.Data
{
[CreateAssetMenu(fileName = "GameElementsPoolData", menuName = "Data/Game Elements Pool Data")]
    public class GameElementsPoolData : ScriptableObject
    {
        [SerializeField] private int _platforms;

        public int Platforms => _platforms;
    }
}