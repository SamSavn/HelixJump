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
            switch (axis)
            {
                case Axis.X:
                    return new Vector3(vector.x + value, vector.y, vector.z);
                case Axis.Y:
                    return new Vector3(vector.x, vector.y + value, vector.z);
                case Axis.Z:
                    return new Vector3(vector.x, vector.y, vector.z + value);
            }

            return vector;
        }
    }
}