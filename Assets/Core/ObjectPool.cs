using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public GameObject gameObject;
    public int poolCount;
    private List<IPoolableObject> objectPool;
    private Transform tr;

    public int Count { get => objectPool.Count; }

    public void InitializePool(Transform transform)
    {
        objectPool = new List<IPoolableObject>();
        tr = transform;

        for (int i = 0; i < poolCount; i++)
        {
            GameObject instantiatedGO = GameObject.Instantiate(gameObject, tr);
            IPoolableObject poolObject = instantiatedGO.GetComponent<IPoolableObject>();
            objectPool.Add(poolObject);
            poolObject.SetOriginatingPool(this);
            instantiatedGO.SetActive(false);
        }

        Debug.Log("" + gameObject.name + " Pool Initialized with count of " + objectPool.Count);
    }

    public IPoolableObject GetObjectFromPool(Transform transform) { return GetObjectFromPool(transform.position, transform.rotation); }
    public IPoolableObject GetObjectFromPool(Vector3 atPosition, Quaternion withRotation)
    {
        if (objectPool.Count > 0)
        {
            // Pop object from pool
            IPoolableObject poolObject = objectPool[0];
            objectPool.Remove(objectPool[0]);

            // Teleport to location in correct direction
            poolObject.GetGameObject().SetActive(true);
            poolObject.GetGameObject().transform.parent = null;
            poolObject.GetGameObject().transform.position = atPosition;
            poolObject.GetGameObject().transform.rotation = withRotation;

            return poolObject;
        }
        else
        {
            return null;
        }
    }

    public void ReturnObjectToPool(IPoolableObject returnedPoolObject)
    {
        objectPool.Add(returnedPoolObject);
        returnedPoolObject.GetGameObject().transform.SetParent(tr);
        returnedPoolObject.GetGameObject().transform.position = Vector3.zero;
        returnedPoolObject.GetGameObject().transform.rotation = Quaternion.identity;
        returnedPoolObject.GetGameObject().SetActive(false);
    }
}