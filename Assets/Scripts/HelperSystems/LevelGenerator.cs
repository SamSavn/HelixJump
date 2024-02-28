using System.Collections.Generic;
using UnityEngine;
using LKS.Data;
using LKS.Gameplay;
using LKS.Managers;

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

        private GameObject _platformPrefab;
        private GameObject _platformClone;
        private Platform _newPlatform;
#endregion

#region Constructors
        public LevelGenerator(LevelGenerationData levelGenerationData)
        {
            _levelGenerationData = levelGenerationData;
            _platformPrefab = AddressablesLoader.LoadSingle<GameObject>(PLATFORM_PREFAB_ADDRESS);
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
            float platformHeight = -index * _levelGenerationData.PlatformsDistance;
            _platformPosition = new Vector3(0f, platformHeight, 0f);

            float randomRotationAngle = Random.Range(0f, 360f);
            _platformRotation = new Vector3(0f, randomRotationAngle, 0f);

            _platformClone = GameObject.Instantiate(_platformPrefab, _tower.transform);
            _newPlatform = _platformClone.GetComponent<Platform>();
            _newPlatform.transform.SetLocalPositionAndRotation(_platformPosition, Quaternion.Euler(_platformRotation));
            _newPlatform.Initialize(randomizationFactor: .5f);

            _level.Add(_newPlatform);
        }
#endregion
    }
}