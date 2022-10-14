using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    public float _FloatTime = .2f;
    public float _FloatMagnitude = .2f;
    public float _PositionOffset = .65f;

    void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, ((Mathf.Sin(Time.time * _FloatTime) * _FloatMagnitude) + _PositionOffset), transform.localPosition.z);
    }
}
