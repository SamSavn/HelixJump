using UnityEngine;

namespace LKS.Extentions
{
    public static class VectorsExtentions
    {
        public static Vector3 LocalToGlobal(this Vector3 local, Transform parent)
        {
            return parent.TransformPoint(local);
        }
    }
}