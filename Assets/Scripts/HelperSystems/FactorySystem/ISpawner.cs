using System;
using System.Collections.Generic;
using UnityEngine;

namespace LKS.AssetsManagement
{
    public interface ISpawner : IDisposable
    {
        bool TrySpawn<T>(out T obj) where T : Component;
        bool TrySpawnAll<T>(out List<T> obj) where T : Component;
    }
}