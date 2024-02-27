using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            return value.IsInRange(min, max, inclusive);
        }

        public static bool IsInCollectionRange<T>(this int value, IEnumerable<T> collection)
        {
            return value.IsInRange(0, collection.Count() - 1);
        }
    }
}