using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        string key = prefab.name;
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        if (poolDictionary[key].Count == 0)
        {
            GameObject obj = Instantiate(prefab);
            poolDictionary[key].Enqueue(obj);
        }

        GameObject pooledObject = poolDictionary[key].Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }

    public void ReturnToPool(GameObject obj)
    {
        string key = obj.name;
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        poolDictionary[key].Enqueue(obj);
        obj.SetActive(false);
    }


    public void ClearPool(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag].Clear();
        }
    }

    public void ClearAllPools()
    {
        foreach (var pair in poolDictionary)
        {
            pair.Value.Clear();
        }
    }
}
