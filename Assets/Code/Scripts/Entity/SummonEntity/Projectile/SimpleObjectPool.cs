using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SimpleObjectPool : MonoBehaviour
{
    public static SimpleObjectPool Instance { get; private set; }

    private Dictionary<GameObject, Stack<GameObject>> poolDict = new Dictionary<GameObject, Stack<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (!poolDict.ContainsKey(prefab))
        {
            poolDict[prefab] = new Stack<GameObject>();
        }

        if (poolDict[prefab].Count > 0)
        {
            GameObject obj =  poolDict[prefab].Pop();
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(prefab);
        
        newObj.transform.SetParent(this.transform);
        newObj.SetActive(true);
        
        newObj.AddComponent<PooledObject>().originalPrefab =  prefab;
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        
        PooledObject pObj = obj.GetComponent<PooledObject>();
        if (pObj != null)
        {
            poolDict[pObj.originalPrefab].Push(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}

public class PooledObject : MonoBehaviour
{
    public GameObject originalPrefab;
}
