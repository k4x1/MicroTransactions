using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> pool;
    private Transform parent;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.pool = new Queue<GameObject>(initialSize);
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
