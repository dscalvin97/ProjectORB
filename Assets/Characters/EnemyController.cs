using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : GameCharacterController, IPoolableObject
{
    protected float _speed = 3;
    private PlayerController _player;
    
    protected GameObject _mesh;

    public override void Start()
    {
        base.Start();
        _player = FindObjectOfType<PlayerController>();
        _mesh = transform.Find("Mesh").gameObject;
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        _characterRigidbody.MovePosition(transform.position + (transform.forward * _speed * Time.deltaTime));
    }

    public virtual void Rotation()
    {
        Vector3 direction = (_player.transform.position - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public override void Die()
    {
        _gameManager.pools[1].ReturnObjectToPool(this);
    }

    public GameObject GetGameObject()
    {
        if (gameObject)
            return gameObject;
        else
            return null;
    }
}
