using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float _CameraSpeed = 5;

    private Vector3 _Offset;
    private GameObject _Player;

    void Start()
    {
        _Offset = transform.position;
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _Player.transform.position + _Offset, Time.deltaTime * _CameraSpeed);
    }
}
