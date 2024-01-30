using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Vector3 CurrentDirection { get; private set; } = Vector2.zero;
    public bool IsInvincible { get; private set; } = false;
    private float _horizontalInput, _verticalInput;
    private bool _ability1Pressed, _ability2Pressed, _ability1Ended, _ability2Ended, _isDodging, _isJumping, _inPushback = false;
    private int _currentSpeed = 8;
    private readonly int _speed = 8, _dodgeMultiplier = 2, _pushbackMultiplier = 2;
    private readonly float _movementSmoothing = 0f;
    private Vector2 _currentVelocity = Vector2.zero;
    private Vector3 _tempDirection;
    private BaseAbility _ability1, _ability2;
    private Dictionary<AbilityType, BaseAbility> _abilityMap = new Dictionary<AbilityType, BaseAbility>();
    public UnityEvent triggerScreenShake = new UnityEvent();
    [SerializeField] private AbilityType _ability1Type = AbilityType.Dodge;
    [SerializeField] private AbilityType _ability2Type = AbilityType.Scratch;
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private Animator _spriteAnimator;

    public void SetAbilities(AbilityType[] abilities)
    {
        _ability1Type = abilities[0];
        _ability2Type = abilities[1];
    }

    public void PerformDodge()
    {
        _isDodging = true;
        IsInvincible = true;
        _currentSpeed *= _dodgeMultiplier;
    }

    public void EndDodge()
    {
        _isDodging = false;
        IsInvincible = false;
        _currentSpeed = _speed;
    }

    public void PerformJump()
    {
        _isJumping = true;
        _horizontalInput = 0;
        _verticalInput = 0;
        _currentSpeed = 0;
    }

    public void EndJump()
    {
        _isJumping = false;
        _currentSpeed = _speed;
    }

    public void TakePushback(float time, Vector3 direction)
    {
        _inPushback = true;
        _tempDirection = CurrentDirection;
        CurrentDirection = direction;
        _currentSpeed *= _pushbackMultiplier;
        triggerScreenShake.Invoke();
        TimeManager.Instance.DoSlowmotion(.05f, 2f);
        StartCoroutine(EndPushback(time));
    }

    private IEnumerator EndPushback(float time)
    {
        yield return new WaitForSeconds(time);
        _inPushback = false;
        _currentSpeed = _speed;
        CurrentDirection = _tempDirection;
    }

    private void Awake()
    {
        BaseAbility[] abilities = GetComponentsInChildren<BaseAbility>();
        foreach (BaseAbility ability in abilities)
        {
            _abilityMap.Add(ability.AbilityType, ability);
        }

        var currentAbilities = DataManager.Instance.GetAbilities();
        _ability1Type = currentAbilities[0];
        _ability2Type = currentAbilities[1];
        SetAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Ability1"))
            _ability1Ended = true;

        if (Input.GetButtonUp("Ability2"))
            _ability2Ended = true;

        if (_isDodging || _isJumping || _inPushback || TimeManager.Instance.IsPaused)
            return;

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        SetSpriteAnimations(_horizontalInput, _verticalInput);
        
        if (Input.GetButtonDown("Ability1"))
            _ability1Pressed = true;

        if (Input.GetButtonDown("Ability2"))
            _ability2Pressed = true;
    }

    private void FixedUpdate()
    {
        Vector3 targetDirection = new Vector3(_horizontalInput, _verticalInput, 0).normalized;
        if (_isDodging || _isJumping || _inPushback)
            targetDirection = CurrentDirection;

        Move(targetDirection);

        if (_ability1Pressed)
            ActivateAbility1();

        if (_ability2Pressed)
            ActivateAbility2();

        if (_ability1Ended)
            EndAbility1();

        if (_ability2Ended)
            EndAbility2();
    }

    private void SetSpriteAnimations(float horizontalInput, float verticalInput)
    {
        // Animator updates; does not rewrite direction on zero to avoid changing sprite direction when input is released
        if (_horizontalInput != 0 || _verticalInput != 0)
        {
            _spriteAnimator.SetBool("isWalking", true);
            _spriteAnimator.SetFloat("XInput", horizontalInput);
            _spriteAnimator.SetFloat("YInput", verticalInput);
        }
        else
        {
            _spriteAnimator.SetBool("isWalking", false);
        }
    }

    private void Move(Vector3 targetDirection)
    {
        Vector3 targetVelocity = targetDirection * _currentSpeed;
        if (targetDirection.magnitude > 0)
        {
            CurrentDirection = targetDirection;
        }

        _playerRB.velocity = Vector2.SmoothDamp(_playerRB.velocity, targetVelocity, ref _currentVelocity, _movementSmoothing);
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

    private void EndAbility1()
    {
        _ability1Ended = false;
        if (_ability1 != null)
            _ability1.EndAbility();
    }

    private void EndAbility2()
    {
        _ability2Ended = false;
        if (_ability2 != null)
            _ability2.EndAbility();
    }
}
