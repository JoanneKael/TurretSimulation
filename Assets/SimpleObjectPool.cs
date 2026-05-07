using UnityEngine;
using System.Collections.Generic;

public class SimpleObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 30;

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }
    public GameObject GetObject()
    {
        if (objectPool.Count > 0)
        {
            //꺼내기 Dequeue
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(true);
            return obj;

            // Debug.Log("pool 객체가 모잘라서 추가 pool 필요");
            // return null; 
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}
