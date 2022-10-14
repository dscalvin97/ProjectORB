using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : GameCharacterController
{
    #region Variables
    // Player Stats
    public int _Level = 1;
    public float _XPToNextLevel = 10;
    public float _CritChance = .05f;
    public float _CritDamageMultiplier = 2f;
    public float _Armor = 10;
    public float _ArmorDamageReduction = .75f;
    public float _DeathTime = 2;
    // public float _CharacterSpeed = 20; (inherited from GameCharacterController)

    public float _CurrentXP { get => _currentXP;
        set {
            _currentXP = value;
            UpdatePlayerLevel();
            if (_gameManager != null)
            {
                _gameManager._XPBar.maxValue = _XPToNextLevel;
                _gameManager._XPBar.value = _currentXP;
            }
        }
    }

    private float _currentXP = 0;

    // Weapon Stats
    public float _FireRate = .5f;
    public float _AmmoSpeed = 10;

    // Player Object references
    public GameObject _WeaponsLeft;
    public GameObject _WeaponsRight;
    public GameObject _thrusterFront;
    public GameObject _thrusterBack;
    public GameObject _thrusterLeft;
    public GameObject _thrusterRight;

    private float _deathTimer;
    private GameControls _gameControls;
    private PlayerInput _playerInput;
    private AudioSource _thrusterAudio;

    private RaycastHit mouseTraceHit = new RaycastHit();

    protected bool _useMouseRotation = true;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _thrusterAudio = GetComponentInChildren<AudioSource>();
        _useMouseRotation = _playerInput.currentControlScheme == "Keyboard and Mouse";
    }

    protected override void Start()
    {
        base.Start();
        _CurrentHealth = _MaxHealth;
        if (_gameManager)
        {
            _gameControls = _gameManager._GameControls;
            _gameControls.Gameplay.PauseGame.started += ToggleGamePauseState;
            _gameManager._HealthBar.maxValue = _MaxHealth;
            _gameManager._HealthBar.value = _CurrentHealth;
            _gameManager._XPBar.maxValue = _XPToNextLevel;
            _gameManager._XPBar.value = _CurrentXP;
        }
        RefreshXPToNextLevel();
    }

    private void Update()
    {
        if (_gameControls != null)
        {
            if (_gameControls.Gameplay.Shoot.inProgress)
            {
                bool isCritAttack = Random.Range(0f, 1f) <= _CritChance;
                for (int i = 0; i < _weapons.Count; i++)
                    _weapons[i].Fire(isCritAttack ? _BaseDamage * _CritDamageMultiplier : _BaseDamage, _FireRate, _AmmoSpeed);
            }

            // Move thrusters based on velocity and character rotation
            Vector3 movement = (PlayerMovement() * _CharacterSpeed);
            
            _thrusterAudio.pitch = Mathf.Lerp(1, 1.25f, PlayerMovement().magnitude);

            ThrusterMovement(_thrusterBack, movement, transform.forward, true, true);
            ThrusterMovement(_thrusterFront, movement, -transform.forward, true, false);
            ThrusterMovement(_thrusterLeft, movement, -transform.right, false, false);
            ThrusterMovement(_thrusterRight, movement, transform.right, false, true);
        }

        if (_isDying)
        {
            _deathTimer += Time.deltaTime;
            if (_deathTimer > _DeathTime)
                Die();
        }
    }

    private void FixedUpdate()
    {
        if (_gameControls != null)
        {
            Movement(PlayerMovement());

            Rotation(PlayerRotation(out mouseTraceHit));
            
            WeaponRotation(_useMouseRotation, GetRotationInput(), mouseTraceHit);
        }
    }
    
    private void OnControlsChanged()
    {
        if (_playerInput)
            _useMouseRotation = _playerInput.currentControlScheme == "Keyboard and Mouse";
    }

    private void OnDestroy()
    {

        _gameControls.Gameplay.PauseGame.started -= ToggleGamePauseState;
    }
    #endregion

    #region Methods Inherited from Parent Class
    public override void DoDamage(float damageAmount)
    {
        base.DoDamage(damageAmount < _Armor ? damageAmount * _ArmorDamageReduction : damageAmount);
    }

    protected override void TakeDamage()
    {
        if (_gameManager._HealthBar != null)
            _gameManager._HealthBar.value = _CurrentHealth;
        else
            throw new Exception("Health bar not found");
    }

    protected override void Die()
    {
        base.Die();
        _gameManager.ShowGameEndScreen();
        _gameManager.PauseGame(false);
    }
    #endregion

    #region Internal Functions
    private Vector3 PlayerMovement()
    {
        Vector2 moveDirection = _gameControls.Gameplay.Move.ReadValue<Vector2>();
        return new Vector3(moveDirection.x, 0, moveDirection.y).normalized;
    }

    private Vector3 PlayerRotation(out RaycastHit hit)
    {
        // Rotation Calculation
        Vector3 desiredDirectionVector = Vector3.zero;


        if (_useMouseRotation)
        {
            if (Physics.Raycast(_cam.ScreenPointToRay(GetRotationInput()), out hit, 100, LayerMask.GetMask("AimDetection")))
                desiredDirectionVector = (new Vector3(hit.point.x, 0, hit.point.z) - transform.position).normalized;
        }
        else
        {
            hit = new RaycastHit();
            Vector3 rotationInput = GetRotationInput();
            desiredDirectionVector = new Vector3(rotationInput.x, 0, rotationInput.y).normalized;
        }

        return desiredDirectionVector;
    }

    private Vector3 GetRotationInput()
    {
        if (_useMouseRotation)
            return _gameControls.Gameplay.LookMouse.ReadValue<Vector2>();
        else
            return _gameControls.Gameplay.LookGamepad.ReadValue<Vector2>();
    }

    private void WeaponRotation(bool useMouseRotation, Vector3 rotationInput, RaycastHit hit)
    {
        Vector3 desiredAimPosition = Vector3.zero;

        if (useMouseRotation)
        {
            desiredAimPosition = hit.point;

            Vector3 desiredLeftWeaponDirection = desiredAimPosition - _WeaponsLeft.transform.position;
            _WeaponsLeft.transform.rotation = Quaternion.LookRotation(desiredLeftWeaponDirection, Vector3.up);
            _WeaponsLeft.transform.localEulerAngles = new Vector3(0, UtilityFunctions.ClampAngle(_WeaponsLeft.transform.localEulerAngles.y , -22.5f, 0f), 0);


            Vector3 desiredRightWeaponDirection = desiredAimPosition - _WeaponsRight.transform.position;
            _WeaponsRight.transform.rotation = Quaternion.LookRotation(desiredRightWeaponDirection, Vector3.up);
            _WeaponsRight.transform.localEulerAngles = new Vector3(0, UtilityFunctions.ClampAngle(_WeaponsRight.transform.localEulerAngles.y, 0f, 22.5f), 0);
        }
        else
        {
            RaycastHit sphereRaycastHit;

            if (Physics.SphereCast(transform.position, 3f, rotationInput, out sphereRaycastHit, 20, LayerMask.GetMask("Default")))
            {
                desiredAimPosition = sphereRaycastHit.point;

                Vector3 desiredLeftWeaponDirection = desiredAimPosition - _WeaponsLeft.transform.position;
                _WeaponsLeft.transform.rotation = Quaternion.LookRotation(desiredLeftWeaponDirection, Vector3.up);
                _WeaponsLeft.transform.localEulerAngles = new Vector3(0, UtilityFunctions.ClampAngle(_WeaponsLeft.transform.localEulerAngles.y, -22.5f, 0f), 0);


                Vector3 desiredRightWeaponDirection = desiredAimPosition - _WeaponsRight.transform.position;
                _WeaponsRight.transform.rotation = Quaternion.LookRotation(desiredRightWeaponDirection, Vector3.up);
                _WeaponsRight.transform.localEulerAngles = new Vector3(0, UtilityFunctions.ClampAngle(_WeaponsRight.transform.localEulerAngles.y, 0f, 22.5f), 0);
            }
        }
    }

    private void ThrusterMovement(GameObject thrusterObject, Vector3 movement, Vector3 referenceVector, bool moveInForwardAxis, bool moveForwardOrRight)
    {
        float thrust = Mathf.Clamp01(Vector3.Dot(movement, referenceVector));

        thrust = Mathf.Lerp(moveForwardOrRight ? .5f : -.5f, 0, thrust);
        Vector3 newPosition = moveInForwardAxis ? new Vector3(0, 0, thrust) : new Vector3(thrust, 0, 0);
        
        thrusterObject.transform.localPosition = Vector3.Lerp(thrusterObject.transform.localPosition, newPosition, Time.deltaTime * 10);
    }

    public void GiveXP(float xpGained)
    {
        _CurrentXP += xpGained;
    }

    private void UpdatePlayerLevel()
    {
        if (_currentXP >= _XPToNextLevel)
        {
            _Level++;
            _currentXP -= _XPToNextLevel;
            // y = (log(x) * (log(x)^2) * (pi^2) + 10) * log(x/2) + 13
            RefreshXPToNextLevel();

            _gameManager.ShowUpgradeChoices();
        }
    }

    private void RefreshXPToNextLevel()
    {
        _XPToNextLevel = Mathf.Pow((_Level) / 0.3f, 2);
    }

    private void ToggleGamePauseState(InputAction.CallbackContext obj)
    {
        if (Time.timeScale == 0)
        {
            _gameManager._PauseMenu.SetActive(false);
            _gameManager.ResumeGame();
        }
        else
        {
            _gameManager._PauseMenu.gameObject.SetActive(true);
            _gameManager.PauseGame(true);
        }
    }
    #endregion
}
