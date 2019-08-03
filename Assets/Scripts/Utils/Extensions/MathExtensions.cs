using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Utils.Extensions
{
    public static class MathExtensions
    {
        public static Vector3? GetClosest(this Vector3 origin, IEnumerable<Vector3> targets){
            Vector3? closest = null;
            float shortest = float.MaxValue;
            foreach (var v in targets) {
                var d = Vector3.SqrMagnitude(v - origin);
                if(d < shortest){
                    closest = v;
                    shortest = d;
                }
            }
            return closest;
        }

        /// <summary>
        /// Checks if the index selected is not out of bounds of the array
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IndexInRange<T>(this List<T> list, int index){
            if (index < 0) {
                return false;
            }

            if (index >= list.Count) {
                return false;
            }

            return true;
        }

        
    }
}