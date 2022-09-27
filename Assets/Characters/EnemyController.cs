using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour, IDamageable, IPoolableObject
{
    private float _health = 10;
    private float _speed = 3;
    private PlayerController _player;
    private GameManager _gameManager;
    
    protected Rigidbody _rig;
    protected GameObject _mesh;

    public float Health { 
        get => _health;
        set
        {
            _health = value;
            if (_health < 0)
                Die();
        }
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _rig = GetComponent<Rigidbody>();
        _mesh = transform.Find("Mesh").gameObject;
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        _rig.MovePosition(transform.position + (transform.forward * _speed * Time.deltaTime));
    }

    public virtual void Rotation()
    {
        Vector3 direction = (_player.transform.position - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Die()
    {
        _gameManager.pools[1].ReturnObjectToPool(this);
    }

    public void DoDamage(float damageAmount)
    {
        Health -= damageAmount;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
