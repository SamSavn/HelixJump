using LKS.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace LKS.GameUpdate
{
    public class GameUpdateController : MonoBehaviour, IDisposable
    {
        private List<IUpdatable> _updatables = new ();
        private IUpdatable _updatable;

        private int _updatableCount = 0;
        private bool _canUpdate;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Dispose();
        }

        private void Update()
        {
            if (!_canUpdate)
                return;

            for (int i = 0; i < _updatableCount; i++)
            {
                _updatable = _updatables[i];

                if (_updatable == null)
                    continue;

                _updatable.CustomUpdate();
            }
        }

        private void OnDestroy()
        {
            _canUpdate = false;
            Dispose();
        }

        public void AddUpdatable(IUpdatable updatable)
        {
            if (!_updatables.Contains(updatable))
            {
                _updatables.Add(updatable);
                _updatableCount++;
            }

            _canUpdate = _updatableCount > 0;
        }

        public void RemoveUpdatable(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable))
            {
                _canUpdate = false;
                _updatables.Remove(updatable);
                _updatableCount--;
            }

            _canUpdate = _updatableCount > 0;
        }

        public void Dispose()
        {
            _updatables.Clear();
            _canUpdate = false;
        }
    }
}