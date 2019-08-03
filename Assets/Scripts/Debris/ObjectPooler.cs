using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FO.Utilities
{

    [CreateAssetMenu]
    public class ObjectPooler : ScriptableObject
    {
        [System.NonSerialized] public Transform objectsParent;
        
        public ObjectHolder[] objects;
        Dictionary<string, ObjectHolder> objDict = new Dictionary<string, ObjectHolder>();
        
        public void Init()
        {
            objectsParent = new GameObject("object pool").transform;
            
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].Init(this);
                if (!objDict.ContainsKey(objects[i].objectName))
                {
                    objDict.Add(objects[i].objectName, objects[i]);
                }
            }    
            
        }

        public GameObject GetObject(string target)
        {
            GameObject retVal = null;

            ObjectHolder holder = null;
            objDict.TryGetValue(target, out holder);

            if (holder != null)
            {
                retVal = holder.GetObject();
                retVal.SetActive(true);
            }
            return retVal;
        }
    }

    [System.Serializable]
    public class ObjectHolder
    {
        public string objectName;
        public GameObject prefab;
        [System.NonSerialized] private int index = 0;
        public int budget;
        [System.NonSerialized] public List<GameObject> objs = new List<GameObject>();

        public void Init(ObjectPooler pool)
        {
            index = 0;
            for (int i = 0; i < budget; i++)
            {
                GameObject go = GameObject.Instantiate(prefab, pool.objectsParent, true);
                go.SetActive(false);
                objs.Add(go);
            }
        }

        public GameObject GetObject()
        {
            GameObject retVal = objs[index];
            index++;

            if (index > objs.Count - 1)
            {
                index = 0;
            }

            
            return retVal;
        }
    }
}
