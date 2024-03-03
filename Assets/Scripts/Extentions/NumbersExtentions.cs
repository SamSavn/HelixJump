using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace LKS.Extentions
{
    public static class NumbersExtentions
    {
        public static bool IsInRange(this float value, float min, float max, bool inclusive = true)
        {
            if (inclusive)
            {
                return value >= min && value <= max;
            }

            return value > min && value < max;
        }

        public static bool IsInRange(this int value, int min, int max, bool inclusive = true)
        {
            return ((float)value).IsInRange(min, max, inclusive);
        }

        public static bool IsInCollectionRange<T>(this int value, IEnumerable<T> collection)
        {
            return value.IsInRange(0, collection.Count() - 1);
        }

        public static float LocalToGlobal(this float value, Axis axis, Transform parent)
        {
            Vector3 local;

            switch (axis)
            {
                case Axis.X:
                    local = new Vector3(value, 0, 0);
                    return local.LocalToGlobal(parent).x;
                case Axis.Y:
                    local = new Vector3(0, value, 0);
                    return local.LocalToGlobal(parent).y;
                case Axis.Z:
                    local = new Vector3(0, 0, value);
                    return local.LocalToGlobal(parent).z;
            }

            return 0;
        }
    }
}