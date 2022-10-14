using UnityEngine;

public interface IPoolableObject
{
    GameObject GetGameObject();

    void SendBackToPool();

    void SetOriginatingPool(ObjectPool originalPool);
}
