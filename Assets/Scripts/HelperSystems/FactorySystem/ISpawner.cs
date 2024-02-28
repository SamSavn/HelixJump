using System;

namespace LKS.AssetsManagement
{
    public interface ISpawner : IDisposable
    {
        bool TrySpawn<T>(out T obj) where T : class;
    }
}