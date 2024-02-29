using LKS.Data;
using LKS.GameElements;
using LKS.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.Helpers
{
    public class LevelGenerator
    {
#region Constants & Fields
        private const string PLATFORM_PREFAB_ADDRESS = "Platform";        

        private LevelGenerationData _levelGenerationData;
        private Tower _tower;

        private List<Platform> _level = new ();

        private Vector3 _platformPosition;
        private Vector3 _platformRotation;

        private Platform _newPlatform;

        private float _platformHeight;
        private float _randomRotationAngle;
#endregion

#region Constructors
        public LevelGenerator(LevelGenerationData levelGenerationData)
        {
            _levelGenerationData = levelGenerationData;
        }
#endregion

#region Public Methods
        public List<Platform> Generate(Tower tower, int levels)
        {
            _tower = tower;

            for (int i = 0; i < levels; i++)
            {
                GeneratePlaform(i);
            }

            return _level;
        }
#endregion

#region Private Methods
        private void GeneratePlaform(int index)
        {
            _newPlatform = PoolingManager.GetFromPool<Platform>();

            if (_newPlatform == null)
                return;

            _platformHeight = index * _levelGenerationData.PlatformsDistance;
            _platformPosition = new Vector3(0f, -_platformHeight, 0f);

            _randomRotationAngle = Random.Range(0f, 360f);
            _platformRotation = new Vector3(0f, _randomRotationAngle, 0f);

            _newPlatform.transform.SetParent(_tower.transform, false);
            _newPlatform.transform.SetLocalPositionAndRotation(_platformPosition, Quaternion.Euler(_platformRotation));
            _newPlatform.Initialize(randomizationFactor: .5f);

            _level.Add(_newPlatform);
        }
#endregion
    }
}