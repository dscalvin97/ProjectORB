using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public GameObject gameObject;
    public int count;
    private List<IPoolableObject> objects;
    private Transform tr;

    public void InitializePool(Transform transform)
    {
        objects = new List<IPoolableObject>();
        tr = transform;

        for (int i = 0; i < count; i++)
        {
            GameObject instantiatedGO = GameObject.Instantiate(gameObject, tr);

            objects.Add(instantiatedGO.GetComponent<IPoolableObject>());
            instantiatedGO.SetActive(false);
        }

        Debug.Log("" + gameObject.name + " Pool Initialized with count of " + objects.Count);
    }

    public IPoolableObject GetObjectFromPool(Transform transform) { return GetObjectFromPool(transform.position, transform.rotation); }
    public IPoolableObject GetObjectFromPool(Vector3 atPosition, Quaternion withRotation)
    {
        // Pop object from pool
        IPoolableObject poolObject = objects[0];
        objects.Remove(objects[0]);

        // Teleport to location in correct direction
        poolObject.GetGameObject().SetActive(true);
        poolObject.GetGameObject().transform.parent = null;
        poolObject.GetGameObject().transform.position = atPosition;
        poolObject.GetGameObject().transform.rotation = withRotation;

        return poolObject;
    }

    public void ReturnObjectToPool(IPoolableObject returnedPoolObject)
    {
        objects.Add(returnedPoolObject);
        returnedPoolObject.GetGameObject().transform.SetParent(tr);
        returnedPoolObject.GetGameObject().transform.position = Vector3.zero;
        returnedPoolObject.GetGameObject().transform.rotation = Quaternion.identity;
        returnedPoolObject.GetGameObject().SetActive(false);
    }
}