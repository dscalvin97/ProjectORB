using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameCharacterController : MonoBehaviour, IDamageable
{
    // Movement
    public float _CharacterSpeed = 20;
    public float _CharacterRotateSpeed = 10;

    // Stats
    public float _MaxHealth = 100;

    protected Rigidbody _characterRigidbody;
    protected GameManager _gameManager;
    protected Camera _cam;
    protected List<IWeapon> _weapons = new List<IWeapon>();

    private float _currentHealth = 100;

    public float _CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (_currentHealth < 0)
                Die();
        }
    }

    public virtual void Start()
    {
        _characterRigidbody = GetComponent<Rigidbody>();
        _gameManager = FindObjectOfType<GameManager>();
        _cam = Camera.main;
        _weapons = GetComponentsInChildren<IWeapon>().ToList();
    }

    public virtual void Movement(Vector3 input)
    {
        Vector3 movement = Vector3.ClampMagnitude(input * _CharacterSpeed * Time.deltaTime, 1);
        _characterRigidbody.MovePosition(_characterRigidbody.position + movement);
    }

    public virtual void Rotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
            _characterRigidbody.rotation = Quaternion.Lerp(_characterRigidbody.rotation, Quaternion.LookRotation(direction, Vector3.up), _CharacterRotateSpeed * Time.deltaTime);
    }

    public virtual void Die()
    {
        Debug.Log("I am dead");
    }

    public virtual void DoDamage(float damageAmount)
    {
        _CurrentHealth -= damageAmount;
    }
}
