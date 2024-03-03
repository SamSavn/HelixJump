using UnityEngine;
using UnityEngine.Animations;

namespace LKS.Extentions
{
    public static class VectorsExtentions
    {
        public static Vector3 LocalToGlobal(this Vector3 local, Transform parent)
        {
            return parent.TransformPoint(local);
        }

        public static Vector3 AddOnAxis(this Vector3 vector, Axis axis, float value)
        {
            Vector3 v = vector;

            switch (axis)
            {
                case Axis.X:
                    return new Vector3(v.x + value, v.y, v.z);
                case Axis.Y:
                    return new Vector3(v.x, v.y + value, v.z);
                case Axis.Z:
                    return new Vector3(v.x, v.y, v.z + value);
            }

            return v;
        }
    }
}