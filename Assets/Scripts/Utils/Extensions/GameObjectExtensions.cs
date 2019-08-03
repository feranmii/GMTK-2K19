using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Utils.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Finds the nearest GameObject relative to a transform
        /// </summary>
        /// <param name="gos"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static GameObject Nearest(this GameObject[] gos, Transform transform){
            var pos = transform.position;
            GameObject nearest = null;
            float shortest = 0;

            foreach(var go in gos){
                var dist = Vector3.Distance(pos, go.transform.position);
			
                if(nearest == null){
                    nearest = go;
                    shortest = dist;
                }
                else if(dist < shortest) {
                    nearest = go;
                    shortest = dist;
                }
            }
		
            return nearest;
        }	
        
        public static T Nearest<T>(this GameObject go, IEnumerable<T> components) where T : Component{
            var pos = go.transform.position;
            T nearest = null;
            float shortest = 0;

            foreach(var c in components){
                var dist = Vector3.Distance(pos, c.gameObject.transform.position);
			
                if(nearest == null){
                    nearest = c;
                    shortest = dist;
                }
                else if(dist < shortest) {
                    nearest = c;
                    shortest = dist;
                }
            }
		
            return nearest;
        }

        /// <summary>
        /// Hides all the renderer under a GameObject
        /// </summary>
        /// <param name="parent"></param>
        public static void HideAll (this Transform parent) {
            Renderer[] tempRenderers = parent.GetComponentsInChildren<Renderer>();
            foreach (Renderer tempRenderer in tempRenderers){
                tempRenderer.enabled = false;
            }
        }
        
        /// <summary>
        /// Enables all the renderers under a GameObject
        /// </summary>
        /// <param name="parent"></param>
        public static void ShowAll (this Transform parent) {
            Renderer[] tempRenderers = parent.GetComponentsInChildren<Renderer>();
            foreach (Renderer tempRenderer in tempRenderers) {
                tempRenderer.enabled = true;
            }
        }

        /// <summary>
        /// Returns true if a renderer is visible from a camera
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
        }

        
        public static T GetComponentInSelfOrChild<T>(this GameObject go) where T : Component {
            var comp = go.GetComponent<T>();
            if(!comp){
                comp = go.GetComponentInChildren<T>();
            }
            return comp;
        }
        
        public static T GetComponentInSelfOrParent<T>(this GameObject go) where T : Component {
            var comp = go.GetComponent<T>();
            if(!comp){
                if(go.transform.parent){
                    comp = go.transform.parent.GetComponent<T>();
                }
            }
            return comp;
        }
    }
}