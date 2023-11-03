using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolTag
{
    Enemy,
    Ammo,
    Health,
    Garlic,
    Revive,
    Score,
    Bomb
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    private Dictionary<PoolTag, Queue<GameObject>> poolDictionary;

    [System.Serializable]
    public class Pool
    {
        public PoolTag tag;
        public GameObject prefab;
        public int size = 10;
    }

    public List<Pool> pools;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        poolDictionary = new Dictionary<PoolTag, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, new(0,0,0), Quaternion.identity);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    public List<GameObject> SetActiveObjects(PoolTag tag, int count)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return new List<GameObject>();
        }


        List<GameObject> objectsToSetActive = new();

        for (int i = 0; i < count; i++)
        {
            if (poolDictionary[tag].Count > 0)
            {
                GameObject objectToSetActive = poolDictionary[tag].Dequeue();
                objectToSetActive.SetActive(true);
                objectsToSetActive.Add(objectToSetActive);
            }
            else
            {
                break;
            }
        }

        return objectsToSetActive;
    }

    public void Destroy(GameObject objectToDestroy, float delay)
    {
        StartCoroutine(DestroyCoroutine(objectToDestroy, delay));
    }

    public void DestroyObject(GameObject objectToDestroy)
    {
        objectToDestroy.SetActive(false);

        PoolTag objectTag = objectToDestroy.GetComponent<PoolableObject>().poolTag;

        if (poolDictionary.ContainsKey(objectTag))
        {
            poolDictionary[objectTag].Enqueue(objectToDestroy);
        }
        else
        {
            Debug.Log("The object's tag does not match any pool. Make sure the object has the PoolableObject component and the tag is set correctly.");
        }

    }

    private IEnumerator DestroyCoroutine(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

        DestroyObject(objectToDestroy);
    }
}
