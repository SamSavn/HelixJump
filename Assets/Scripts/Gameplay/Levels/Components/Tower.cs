using LKS.Data;
using LKS.Extentions;
using LKS.Helpers;
using LKS.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.Gameplay
{
    public class Tower : MonoBehaviour
    {
#region Serialized Fields
        [SerializeField] private LevelGenerationData generationData;
#endregion

#region Fields
        private const int PLATFORMS_OFFSET = 3;
        private LevelGenerator _levelGenerator;
        private List<Platform> _platforms = new List<Platform>();
        private float _position;
        private float _height;
        #endregion

        #region Properties
        private float Offset => PLATFORMS_OFFSET * generationData.PlatformsDistance * 10f;
        private float TopPosThreshold => _position + (_height * .5f) + Offset;
        private float BottomPosThreshold => _position - (_height * .5f) - Offset; 
#endregion

#region Unity Methods
        private void Awake()
        {
            _levelGenerator = new LevelGenerator(generationData);
            _position = transform.position.y;
            _height = transform.localScale.y;

            GameManager.SetTower(this);
        }

        private void Start()
        {
            _platforms = _levelGenerator.Generate(this, 10);
        }
#endregion

#region Public Methods
        public bool CanActivate(Platform platform)
        {
            return platform.Position.IsInRange(BottomPosThreshold, TopPosThreshold);
        } 
#endregion
    }
}