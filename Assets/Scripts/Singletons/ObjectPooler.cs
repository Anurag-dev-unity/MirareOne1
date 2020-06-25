using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPooler : Singleton<ObjectPooler>
{
    /// <summary>
    /// Pool.
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;

        public Pool(string _tag, GameObject _prefab, int _size)
        {
            tag = _tag;
            prefab = _prefab;
            size = _size;
        }
    }

    /// <summary>
    /// Texture data.
    /// </summary>
    public struct TextureData
    {
        public List<Texture> grayScaleTextures;
        public List<Texture> originalTextures;
        public TextureData(List<Texture> _grayScaleTextures, List<Texture> _originalTextures)
        {
            grayScaleTextures = _grayScaleTextures;
            originalTextures = _originalTextures;
        }
    }

    //public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    public Dictionary<string, TextureData> textureDictionary = new Dictionary<string, TextureData>();
    public GameObject parent;
    private IList<Object> objList;

    /// <summary>
    /// create object pool for all objects loaded from file.
    /// </summary>
    public void Init(string path)
    {
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        Debug.Log("RARO object pool Init");

        GameObject finalObj = null;
        //int index = 0;
        objList = new List<Object>();
        objList = Utils.Load<GameObject>(path);
        Debug.Log("RARO loading data from file");
        if (objList.Count <= 0)
        {
            Debug.LogError("RARO Object load failed");
            return;
        }
        if (poolDictionary.ContainsKey(objList[0].name))
        {
            Debug.LogError("RARO  Key with same name already exsists");
            return;
        }
        //Debug.Log(objList[0].name);
        //for (int i = 0; i < pools.Count; i++)
        //{
        //    if (pools[i].tag == objList[0].name)
        //    {
        //        return;
        //    }
        //}
        //Pool p = new Pool(objList[0].name, null, 20);
        //pools.Add(p);
        //for (int i = 0; i < pools.Count; i++)
        //{
        //    if (pools[i].tag == objList[0].name)
        //    {
        //        index = i;
        //        Debug.Log("index-> " + i);
        //        pools[i].prefab = (GameObject)objList[0];
        //    }
        //}
        //if(poolDictionary.ContainsKey(pools[index].tag))
        //{
        //    Debug.LogError("Key with same name already exsists");
        //    return;
        //}
        parent = GameObject.Find("PooledObjects");
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            if (i == 0)
            {
                GameObject obj = Instantiate((GameObject)objList[0]);
                obj.layer = 8;
                obj.AddComponent<InteractableObject>();
                obj.AddComponent<LeanScale>();
                obj.AddComponent<Rigidbody>().useGravity = false;


                GameObject selMark = Instantiate(AppManager.Instance.selectionMarker);

                // ------
                if (obj.gameObject.name.Contains("brain") ||
                    obj.gameObject.name.Contains("kidney") ||
                    obj.gameObject.name.Contains("lung") ||
                    obj.gameObject.name.Contains("liver") ||
                    obj.gameObject.name.Contains("intestine") ||
                    obj.gameObject.name.Contains("heart") ||
                    obj.gameObject.name.Contains("skeleton"))

                {
                    selMark.transform.position = obj.transform.position + new Vector3(0, -0.25f, 0);
                }
                // ------


                selMark.transform.parent = obj.transform;
                selMark.transform.SetAsFirstSibling();
                //------

                //obj.AddComponent<MeshCollider>().convex = true;
                //obj.GetComponent<MeshCollider>().isTrigger = true;

               //if (!obj.gameObject.name.Contains("car"))
               // {

                    //Debug.Log("RARO nahi table hai ji");

                    if (!textureDictionary.ContainsKey(objList[0].name))
                    {
                        InteractableObject interactable = obj.GetComponent<InteractableObject>();
                        if (interactable.grayScaleTextures.Count <= 0 && interactable.originalTextures.Count <= 0)
                        {
                            List<Texture> _originalTextures = interactable.OriginalTexture();
                            List<Texture> _grayScaleTextures = interactable.GrayScaleTexture();
                            TextureData materialData = new TextureData(_grayScaleTextures, _originalTextures);
                            textureDictionary.Add(objList[0].name, materialData);
                        }
                    }

               // }



                //IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
                //if (pooledObject != null)
                //    pooledObject.Init(objList[0].name);
                Debug.Log("RARO - " + obj.name);
                obj.transform.SetParent(parent.transform);
                obj.SetActive(false);
                finalObj = obj;
                objectPool.Enqueue(obj);
                Debug.Log("RARO pool prefab created");
            }
            else
            {
                GameObject go = Instantiate(finalObj);
                go.transform.SetParent(parent.transform);
                objectPool.Enqueue(go);
                Debug.Log("RARO final Object created");
            }
        }
        poolDictionary.Add(objList[0].name, objectPool);

        stopWatch.Stop();
        Debug.Log("RARO obj pool created-> " + stopWatch.Elapsed.Milliseconds + "ms");
    }
    /// <summary>
    /// spawn object from poolDictionary.
    /// </summary>
    /// <param name="tag">object name in pool</param>
    /// <param name="position">object positon</param>
    /// <param name="rotation">object rotation</param>
    /// <param name="parent">object parent</param>
    /// <returns>gameObject</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.SetParent(parent);
        objectToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    /// <summary>
    /// Gets from pool.
    /// </summary>
    /// <returns>The from pool.</returns>
    /// <param name="tag">Tag.</param>
    /// <param name="parent">Parent.</param>
    public GameObject GetFromPool(string tag, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        Debug.Log("RARO in get from pool");

        GameObject go = poolDictionary[tag].Dequeue();
        Debug.Log("RARO - go - " + go);

        /*
        IPooledObject pooledObject = go.GetComponent<IPooledObject>();
        Debug.Log("RARO - go - 1 " + pooledObject);
        if (pooledObject != null)
        {
            Debug.Log("RARO - go - 2");
            pooledObject.Init(tag);
            Debug.Log("RARO - go - 3");

        }
        else{
            Debug.Log("RARO - go - 4");
        }
*/

        go.transform.SetParent(parent);

        Debug.Log("RARO - go - 5");
        go.SetActive(false);
        Debug.Log("RARO - go - 6");
        poolDictionary[tag].Enqueue(go);
        Debug.Log("RARO - go - 7");
        return go;
    }
}