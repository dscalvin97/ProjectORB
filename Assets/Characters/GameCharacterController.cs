using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameCharacterController : MonoBehaviour, IDamageable
{
    #region Variables
    // Movement
    public float _CharacterSpeed = 20;
    public float _CharacterRotateSpeed = 10;

    // Stats
    public float _BaseDamage = 10;

    protected Rigidbody _characterRigidbody;
    protected GameManager _gameManager;
    protected Camera _cam;
    protected List<IWeapon> _weapons = new List<IWeapon>();
    protected GameObject _characterMesh;
    protected bool _isDying = false;
    [SerializeField]
    private float _maxHealth = 100;
    [SerializeField]
    private float _currentHealth = 100;
    private bool _canDie = true;

    public float _CurrentHealth
    {
        get => _currentHealth;
        set
        {
            var isDamage = value < _currentHealth;
            _currentHealth = value;
            if (isDamage)
                TakeDamage();
            if (_currentHealth < 0 && _canDie)
            {
                _canDie = false;
                StartDying();
            }
        }
    }

    public float _MaxHealth { get => _maxHealth;
        set 
        {
            _maxHealth = value;
            _CurrentHealth = _maxHealth;
        } 
    }
    #endregion

    #region Unity Methods
    protected virtual void Start()
    {
        _characterMesh = transform.Find("Mesh").gameObject;
        _characterRigidbody = GetComponent<Rigidbody>();
        _gameManager = FindObjectOfType<GameManager>();
        _cam = Camera.main;
        _weapons = GetComponentsInChildren<IWeapon>().ToList();
    }

    protected virtual void OnEnable()
    {
        _CurrentHealth = _MaxHealth;
    }
    #endregion

    #region Class Implementation Inherited by Child Classes
    protected virtual void TakeDamage() {}

    protected virtual void Die()
    {
        _isDying = false;
    }
    #endregion

    #region Interface Implementations
    public virtual void StartDying()
    {
        _isDying = true;
    }

    public virtual void DoDamage(float damageAmount)
    {
        _CurrentHealth -= damageAmount;
    }
    #endregion

    #region Internal Functions
    protected void Movement(Vector3 input)
    {
        if (!_isDying)
        {
            Vector3 movement = Vector3.ClampMagnitude(input, 1) * _CharacterSpeed;
            _characterRigidbody.velocity = Vector3.Lerp(_characterRigidbody.velocity, movement, Time.deltaTime * 20);
        }
        else
        {
            _characterRigidbody.velocity = Vector3.Lerp(_characterRigidbody.velocity, Vector3.zero, Time.deltaTime * 2);
        }
    }

    protected void Rotation(Vector3 direction)
    {
        if (!_isDying)
        {
            if (direction != Vector3.zero)
                _characterRigidbody.rotation = Quaternion.Lerp(_characterRigidbody.rotation, Quaternion.LookRotation(direction, Vector3.up), _CharacterRotateSpeed * Time.deltaTime);
        }
    }
    #endregion
}
