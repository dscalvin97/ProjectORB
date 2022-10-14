using UnityEngine;

public class StickToGameObject : MonoBehaviour
{
    public GameObject target;
    public Vector3 _Offset;

    private void Update()
    {
        transform.position = target.transform.position + _Offset;
    }
}
