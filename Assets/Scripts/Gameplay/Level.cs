using LKS.Data;
using LKS.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.Gameplay
{
    public class Level : MonoBehaviour
    {
#region Serialized Fields
        [SerializeField] private LevelGenerationData generationData;
#endregion

#region Fields
        private LevelGenerator _levelGenerator;
        private List<Platform> _platforms = new List<Platform>();
#endregion

#region Unity Methods
        private void Awake()
        {
            _levelGenerator = new LevelGenerator(generationData);
        }

        private void Start()
        {
            _platforms = _levelGenerator.Generate(transform, 10);
        } 
#endregion
    }
}