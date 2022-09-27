using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // Movement
    public float _PlayerSpeed = 20;
    public float _PlayerRotateSpeed = 10;
    
    // Stats
    public float _Speed = 1;
    public float _CurrentHealth = 100;
    public float _MaxHealth = 100;
    public float _CritChance = .5f;
    public float _CritDamage = 1.5f;
    public float _Armor = 10;
    public float _ArmorDamageReduction = .75f;

    private Rigidbody _playerRigidbody;
    private GameManager _gameManager;
    private GameControls _gameControls;
    private PlayerInput _playerInput;
    private bool _useMouseRotation = true;
    private Camera _cam;

    public GameObject _thrusterFront;
    public GameObject _thrusterBack;
    public GameObject _thrusterLeft;
    public GameObject _thrusterRight;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _useMouseRotation = _playerInput.currentControlScheme == "Keyboard and Mouse";
    }

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _gameManager = FindObjectOfType<GameManager>();
        _gameControls = _gameManager._GameControls;
        _cam = Camera.main;
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();

    }

    private void OnControlsChanged()
    {
        if (!_playerInput) return;

        _useMouseRotation = _playerInput.currentControlScheme == "Keyboard and Mouse";
    }

    private void Movement()
    {
        Vector2 direction = _gameControls.Gameplay.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(direction.x, 0, direction.y).normalized;
        Vector3 movement = (inputDirection * _PlayerSpeed * _Speed * Time.deltaTime);
        _playerRigidbody.MovePosition(_playerRigidbody.position + movement);

        // Set thruster flame positions
        float backThrust = Mathf.Clamp01(Vector3.Dot(_playerRigidbody.velocity, transform.forward));
        float frontThrust = Mathf.Clamp01(Vector3.Dot(_playerRigidbody.velocity, -transform.forward));
        float leftThrust = Mathf.Clamp01(Vector3.Dot(_playerRigidbody.velocity, -transform.right));
        float rightThrust = Mathf.Clamp01(Vector3.Dot(_playerRigidbody.velocity, transform.right));

        backThrust = Mathf.Lerp(.5f, 0, backThrust);
        frontThrust = Mathf.Lerp(-.5f, 0, frontThrust);
        leftThrust = Mathf.Lerp(-.5f, 0, leftThrust);
        rightThrust = Mathf.Lerp(.5f, 0, rightThrust);
        _thrusterBack.transform.localPosition = Vector3.Lerp(_thrusterBack.transform.localPosition, new Vector3(0, 0, backThrust), Time.deltaTime * 10);
        _thrusterFront.transform.localPosition = Vector3.Lerp(_thrusterFront.transform.localPosition, new Vector3(0, 0, frontThrust), Time.deltaTime * 10);
        _thrusterLeft.transform.localPosition = Vector3.Lerp(_thrusterLeft.transform.localPosition, new Vector3(leftThrust, 0, 0), Time.deltaTime * 10);
        _thrusterRight.transform.localPosition = Vector3.Lerp(_thrusterRight.transform.localPosition, new Vector3(rightThrust, 0, 0), Time.deltaTime * 10);
    }

    private void Rotation()
    {
        Vector3 direction = Vector3.zero;
        if (_useMouseRotation)
        {
            Vector3 input = _gameControls.Gameplay.LookMouse.ReadValue<Vector2>();
            RaycastHit hit;

            if (Physics.Raycast(_cam.ScreenPointToRay(input), out hit, 100, LayerMask.GetMask("Ground")))
                direction = (hit.point - transform.position).normalized;
        }
        else
        {
            Vector2 input = _gameControls.Gameplay.LookGamepad.ReadValue<Vector2>();
            direction = new Vector3(input.x, 0, input.y).normalized;
        }

        if (direction != Vector3.zero)
            _playerRigidbody.rotation = Quaternion.Lerp(_playerRigidbody.rotation, Quaternion.LookRotation(direction, Vector3.up), _PlayerRotateSpeed * Time.deltaTime);
    }
}
