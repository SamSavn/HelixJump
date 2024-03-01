using LKS.Data;
using LKS.Helpers;
using UnityEngine;

namespace LKS.Managers
{
    public static class LayerMaskManager
    {
#region Properties
        public static LayerMaskData Data { get; private set; }
#endregion

#region Constructors
        static LayerMaskManager()
        {
            Data = AddressablesLoader.LoadSingle<LayerMaskData>("LayerMaskData");
        }
#endregion

#region Public Methods
        public static bool HasLayer(GameObject gameObject, LayerMask layerMask)
        {
            int gameObjectLayer = gameObject.layer;
            return (layerMask & (1 << gameObjectLayer)) != 0;
        }

        public static bool IsPlatformSegment(GameObject gameObject)
        {
            return HasLayer(gameObject, Data.PlatformSegmentMask);
        }

        public static bool IsObstacle(GameObject gameObject)
        {
            return HasLayer(gameObject, Data.ObstacleMask);
        }

        public static bool IsFallZone(GameObject gameObject)
        {
            return HasLayer(gameObject, Data.FallZoneMask);
        } 
#endregion
    }
}