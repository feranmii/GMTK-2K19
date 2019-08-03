using UnityEngine;

namespace Utils.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 RandomVector2(Vector2 arg1, Vector2 arg2)
        {
            Vector2 retVal;
            
            retVal = new Vector2(Random.Range(arg1.x, arg2.x),Random.Range(arg1.y, arg2.y));

            return retVal;
        }
    }
}