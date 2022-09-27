using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour, IWeaponAmmo, IPoolableObject
{
    private float _speed;
    private float _damage;
    private Rigidbody _rig;
    private Camera _cam;
    private GameManager _gameManager;

    private void Start()
    {
        _rig = GetComponent<Rigidbody>();
        _cam = Camera.main;
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        Vector3 vpPos = _cam.WorldToViewportPoint(transform.position);
        float boundsMin = -.25f;
        float boundsMax = 1.25f;
        if (vpPos.x < boundsMin || vpPos.y < boundsMin || vpPos.x > boundsMax || vpPos.y > boundsMax)
            _gameManager.pools[0].ReturnObjectToPool(this);
    }

    private void FixedUpdate()
    {
        _rig.MovePosition(_rig.position + (transform.TransformDirection(Vector3.forward) * Time.deltaTime * _speed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDamageable>().DoDamage(_damage);
        _gameManager.pools[0].ReturnObjectToPool(this);
    }

    public void SetAmmoDamage(float damageValue)
    {
        _damage = damageValue;
    }

    public void SetAmmoSpeed(float speedValue)
    {
        _speed = speedValue;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
