using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace LKS.GameUpdate
{
    public class GameUpdateController : MonoBehaviour, IDisposable
    {
        private HashSet<IUpdatable> _updatablesHash = new ();        
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

            foreach (IUpdatable updatable in _updatablesHash)
            {
                if (updatable == null)
                    continue;

                updatable.Update();
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void AddUpdatable(IUpdatable updatable)
        {
            if (!_updatablesHash.Contains(updatable))
            {
                _updatablesHash.Add(updatable);
            }
        }

        public void RemoveUpdatable(IUpdatable updatable)
        {
            if (_updatablesHash.Contains(updatable))
            {
                _updatablesHash.Remove(updatable);
            }
        }

        public void Dispose()
        {
            _updatablesHash.Clear();
            _canUpdate = false;
        }
    }
}