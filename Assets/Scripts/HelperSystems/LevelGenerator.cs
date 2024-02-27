using LKS.Data;
using LKS.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.Helpers
{
    public class LevelGenerator
    {
#region Constants & Fields
        private const float PLATFORMS_DISTANCE = .5f;
#endregion

#region Fields
        private LevelGenerationData _levelGenerationData;
        private Transform _tower;

        private List<Platform> _level = new ();

        private Vector3 _platformPosition;
        private Vector3 _platformRotation;
#endregion

#region Constructors
        public LevelGenerator(LevelGenerationData levelGenerationData)
        {
            _levelGenerationData = levelGenerationData;
        }
#endregion

#region Public Methods
        public List<Platform> Generate(Transform tower, int levels)
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
            float platformHeight = -index * PLATFORMS_DISTANCE;
            _platformPosition = new Vector3(0f, platformHeight, 0f);

            float randomRotationAngle = Random.Range(0f, 360f);
            _platformRotation = new Vector3(0f, randomRotationAngle, 0f);

            GameObject clone = GameObject.Instantiate(_levelGenerationData.PlatformPrefab, _tower);
            Platform platform = clone.GetComponent<Platform>();
            platform.transform.SetLocalPositionAndRotation(_platformPosition, Quaternion.Euler(_platformRotation));
            platform.Randomize(.5f);

            _level.Add(platform);
        }
#endregion
    }
}