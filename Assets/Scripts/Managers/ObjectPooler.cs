using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolTag
{
    Enemy,
    Ammo,
    Health,
    Garlic,
    Revive
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

        Debug.Log("Object pooler initialized.");
    }

    public List<GameObject> SetActiveObjects(PoolTag tag, int count)
    {
        Debug.Log("Setting Active Objects");
        Debug.Log("Tag: " + tag);
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return new List<GameObject>();
        }


        List<GameObject> objectsToSetActive = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            if (poolDictionary[tag].Count > 0)
            {
                GameObject objectToSetActive = poolDictionary[tag].Dequeue();
                Debug.Log("Setting object active");
                objectToSetActive.SetActive(true);
                objectsToSetActive.Add(objectToSetActive);
            }
            else
            {
                Debug.Log("Not enough objects in the pool to complete the request.");
                break;
            }
        }

        return objectsToSetActive;
    }

    public void Destroy(GameObject objectToDestroy, float delay)
    {
        StartCoroutine(DestroyCoroutine(objectToDestroy, delay));
    }

    private IEnumerator DestroyCoroutine(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

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
}
