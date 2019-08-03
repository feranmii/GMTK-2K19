using UnityEngine;

namespace Helpers
{
    public class UnityHelper : MonoBehaviour
    {
        /*
        /// <summary>
        /// Tries to find an object of the given type, otherwise creates one.
        /// </summary>
        public static T FindOrCreate<T>() where T : Component
        {
            T instance = Object.FindObjectOfType<T>();
            return instance != null ? instance : CreateObject<T>();
        }
*/

        /// <summary>
        /// Creates a <see cref="GameObject"/> with a component of the given type attached.
        /// </summary>
        public static T CreateObject<T>(string name) where T : Component
        {
            var gameObject = new GameObject(name);
            return gameObject.AddComponent<T>();
        }
        
        /// <summary>
        /// Tries to find an object of the given type, otherwise creates one.
        /// </summary>
        public static T FindOrCreate<T>(string name) where T : Component
        {
            T instance = Object.FindObjectOfType<T>();
            return instance != null ? instance : CreateObject<T>(name);
        }
   
    }
}