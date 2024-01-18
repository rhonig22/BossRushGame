using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput, _verticalInput;
    private bool _ability1Pressed, _ability2Pressed, _isDodging = false;
    private int _currentSpeed = 8;
    private readonly int _speed = 8;
    private readonly float _movementSmoothing = 0f;
    private Vector2 _currentVelocity = Vector2.zero;
    private Vector3 _currentDirection = Vector2.zero;
    private BaseAbility _ability1, _ability2;
    private Dictionary<AbilityType, BaseAbility> _abilityMap = new Dictionary<AbilityType, BaseAbility>();
    [SerializeField] private AbilityType _ability1Type = AbilityType.Dodge;
    [SerializeField] private AbilityType _ability2Type = AbilityType.Scratch;
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private Sprite _mouseFront;
    [SerializeField] private Sprite _mouseSide;
    [SerializeField] private Sprite _mouseBack;

    public void PerformDodge()
    {
        _isDodging = true;
        _currentSpeed *= 2;
    }

    public void EndDodge()
    {
        _isDodging = false;
        _currentSpeed = _speed;
    }

    private void Awake()
    {
        BaseAbility[] abilities = GetComponentsInChildren<BaseAbility>();
        foreach (BaseAbility ability in abilities)
        {
            _abilityMap.Add(ability.AbilityType, ability);
        }

        SetAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDodging)
            return;

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Ability1"))
            _ability1Pressed = true;

        if (Input.GetButtonDown("Ability2"))
            _ability2Pressed = true;
    }

    private void FixedUpdate()
    {
        Vector3 targetDirection = new Vector3(_horizontalInput, _verticalInput, 0).normalized;
        if (_isDodging)
            targetDirection = _currentDirection;

        Move(targetDirection);

        if (_ability1Pressed)
            ActivateAbility1();

        if (_ability2Pressed)
            ActivateAbility2();
    }

    private void Move(Vector3 targetDirection)
    {
        Vector3 targetVelocity = targetDirection * _currentSpeed;
        if (targetDirection.magnitude > 0)
        {
            SetSpriteDirection(targetDirection);
            _currentDirection = targetDirection;
        }

        _playerRB.velocity = Vector2.SmoothDamp(_playerRB.velocity, targetVelocity, ref _currentVelocity, _movementSmoothing);
    }

    private void SetSpriteDirection(Vector3 direction)
    {
        if (direction.y > 0 && direction.x == 0)
        {
            _playerSpriteRenderer.sprite = _mouseBack;
        }
        else if (direction.y < 0 && direction.x == 0)
        {
            _playerSpriteRenderer.sprite = _mouseFront;
        }
        else if (direction.x < 0)
        {
            _playerSpriteRenderer.sprite = _mouseSide;
            _playerSpriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            _playerSpriteRenderer.sprite = _mouseSide;
            _playerSpriteRenderer.flipX = false;
        }
    }

    private void SetAbilities()
    {
        _ability1 = _abilityMap[_ability1Type];
        _ability2 = _abilityMap[_ability2Type];
    }

    private void ActivateAbility1()
    {
        _ability1Pressed = false;
        if (_ability1 != null)
            _ability1.ActivateAbility();
    }

    private void ActivateAbility2()
    {
        _ability2Pressed = false;
        if (_ability2 != null)
            _ability2.ActivateAbility();
    }
}
