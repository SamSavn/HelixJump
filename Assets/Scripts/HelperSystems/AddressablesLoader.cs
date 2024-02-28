using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace LKS.AssetsManagement
{
    public static class AddressablesLoader
    {
        public static T LoadSingle<T>(string key)
        {
            IList<IResourceLocation> locations = Addressables.LoadResourceLocationsAsync(key, typeof(T)).WaitForCompletion();

            foreach (var location in locations)
                return Addressables.LoadAssetAsync<T>(location).WaitForCompletion();

            return default(T);
        }

        public static List<T> LoadAll<T>(string key)
        {
            IList<IResourceLocation> locations = Addressables.LoadResourceLocationsAsync(key, typeof(T)).WaitForCompletion();

            List<T> contents = new List<T>();

            foreach (var location in locations)
                contents.Add(Addressables.LoadAssetAsync<T>(location).WaitForCompletion());

            return contents;
        }
    }
}