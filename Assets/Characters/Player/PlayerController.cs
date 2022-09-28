using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : GameCharacterController
{
    // Stats
    public float _CritChance = .5f;
    public float _CritDamage = 1.5f;
    public float _Armor = 10;
    public float _ArmorDamageReduction = .75f;

    public GameObject _thrusterFront;
    public GameObject _thrusterBack;
    public GameObject _thrusterLeft;
    public GameObject _thrusterRight;
    
    protected bool _useMouseRotation = true;

    private GameControls _gameControls;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _useMouseRotation = _playerInput.currentControlScheme == "Keyboard and Mouse";
    }

    public override void Start()
    {
        base.Start();
        _gameControls = _gameManager._GameControls;
    }

    private void Update()
    {
        if (_gameControls.Gameplay.Shoot.inProgress)
        {
            foreach (IWeapon weapon in _weapons)
            {
                weapon.Fire();
            }
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();

        PlayerRotation();
    }

    private void OnControlsChanged()
    {
        if (_playerInput)
            _useMouseRotation = _playerInput.currentControlScheme == "Keyboard and Mouse";
    }

    public override void Movement(Vector3 input)
    {
        Vector3 movement = (input * _CharacterSpeed);
        _characterRigidbody.MovePosition(_characterRigidbody.position + (movement) * Time.deltaTime);

        ThrusterMovement(_thrusterBack, movement, transform.forward, true, true);
        ThrusterMovement(_thrusterFront, movement, -transform.forward, true, false);
        ThrusterMovement(_thrusterLeft, movement, -transform.right, false, false);
        ThrusterMovement(_thrusterRight, movement, transform.right, false, true);
        // Set thruster flame positions
        //float backThrust = Mathf.Clamp01(Vector3.Dot(movement, transform.forward));
        //float frontThrust = Mathf.Clamp01(Vector3.Dot(movement, -transform.forward));
        //float leftThrust = Mathf.Clamp01(Vector3.Dot(movement, -transform.right));
        //float rightThrust = Mathf.Clamp01(Vector3.Dot(movement, transform.right));
        //backThrust = Mathf.Lerp(.5f, 0, backThrust);
        //frontThrust = Mathf.Lerp(-.5f, 0, frontThrust);
        //leftThrust = Mathf.Lerp(-.5f, 0, leftThrust);
        //rightThrust = Mathf.Lerp(.5f, 0, rightThrust);
        //_thrusterBack.transform.localPosition = Vector3.Lerp(_thrusterBack.transform.localPosition, new Vector3(0, 0, backThrust), Time.deltaTime * 10);
        //_thrusterFront.transform.localPosition = Vector3.Lerp(_thrusterFront.transform.localPosition, new Vector3(0, 0, frontThrust), Time.deltaTime * 10);
        //_thrusterLeft.transform.localPosition = Vector3.Lerp(_thrusterLeft.transform.localPosition, new Vector3(leftThrust, 0, 0), Time.deltaTime * 10);
        //_thrusterRight.transform.localPosition = Vector3.Lerp(_thrusterRight.transform.localPosition, new Vector3(rightThrust, 0, 0), Time.deltaTime * 10);
    }

    private void PlayerMovement()
    {
        Vector2 moveDirection = _gameControls.Gameplay.Move.ReadValue<Vector2>();
        Vector3 movementInput = new Vector3(moveDirection.x, 0, moveDirection.y).normalized;

        Movement(movementInput);
    }

    private void PlayerRotation()
    {
        // Rotation Calculation
        Vector3 rotationInput = new Vector3();
        Vector3 rotateDirection = Vector3.zero;

        if (_useMouseRotation)
        {
            rotationInput = _gameControls.Gameplay.LookMouse.ReadValue<Vector2>();

            RaycastHit hit;

            if (Physics.Raycast(_cam.ScreenPointToRay(rotationInput), out hit, 100, LayerMask.GetMask("Ground")))
                rotateDirection = (hit.point - transform.position).normalized;
        }
        else
        {
            rotationInput = _gameControls.Gameplay.LookGamepad.ReadValue<Vector2>();
            rotateDirection = new Vector3(rotationInput.x, 0, rotationInput.y).normalized;
        }

        Rotation(rotateDirection);
    }

    private void ThrusterMovement(GameObject thrusterObject, Vector3 movement, Vector3 referenceVector, bool moveInForwardAxis, bool moveForwardOrRight)
    {
        float thrust = Mathf.Clamp01(Vector3.Dot(movement, referenceVector));
        thrust = Mathf.Lerp(moveForwardOrRight ? .5f : -.5f, 0, thrust);
        Vector3 newPosition = moveInForwardAxis ? new Vector3(0, 0, thrust) : new Vector3(thrust, 0, 0);
        thrusterObject.transform.localPosition = Vector3.Lerp(thrusterObject.transform.localPosition, newPosition, Time.deltaTime * 10);
    }
}
