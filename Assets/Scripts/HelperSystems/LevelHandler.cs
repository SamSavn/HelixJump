using LKS.Data;
using LKS.Extentions;
using LKS.GameElements;
using LKS.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LKS.Helpers
{
    public class LevelHandler : IDisposable
    {
#region Constants & Fields
        private LevelGenerationData _levelGenerationData;
        private Tower _tower;

        private List<Platform> _level = new ();

        private Vector3 _platformPosition;
        private Vector3 _platformRotation;

        private Platform _newPlatform;

        private float _platformPositionY;
        private float _randomRotationAngle;
#endregion

#region Constructors
        public LevelHandler(LevelGenerationData levelGenerationData)
        {
            _levelGenerationData = levelGenerationData;
        }        
#endregion

#region Public Methods
        public List<Platform> Generate(Tower tower, int platforms)
        {
            _tower = tower;

            for (int i = 0; i < platforms; i++)
            {
                GeneratePlaform(i);
            }

            return _level;
        }

        public void UpdateLevel(out Platform newPlatform)
        {
            if (_level == null || _level.Count == 0)
            {
                newPlatform = null;
                return;
            }

            PoolingManager.AddToPool(_level[0]);
            _level.RemoveAt(0);

            GeneratePlaform(_level.Count);
            newPlatform = _level[_level.Count - 1];
        }

        public void Dispose()
        {
            _level.Clear();
            _newPlatform = null;
            _levelGenerationData = null;
            _tower = null;
        }
#endregion

#region Private Methods
        private void GeneratePlaform(int index)
        {
            _newPlatform = PoolingManager.GetFromPool<Platform>();

            if (_newPlatform == null)
                return;

            if (_level.Count > 0)
            {
                _platformPositionY = _level[^1].LocalPosition.y - _levelGenerationData.PlatformsDistance;
            }
            else
            {
                _platformPositionY = 0;
            }

            _platformPosition = new Vector3(0f, _platformPositionY, 0f);

            _randomRotationAngle = index == 0 ? 0 : Random.Range(0f, 360f);
            _platformRotation = new Vector3(0f, _randomRotationAngle, 0f);

            _newPlatform.transform.SetParent(_tower.transform, false);
            _newPlatform.transform.SetLocalPositionAndRotation(_platformPosition, Quaternion.Euler(_platformRotation));
            _newPlatform.LocalScale = Vector3.one;            
            _newPlatform.Initialize(index, _tower, _levelGenerationData);

            _level.Add(_newPlatform);
        }
#endregion
    }
}