using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Vector3 CurrentDirection { get; private set; } = Vector2.up;
    public bool IsInvincible { get; private set; } = false;
    private float _horizontalInput, _verticalInput;
    private bool _ability1Pressed, _ability2Pressed, _ability1Ended, _ability2Ended, _isDodging, _noMovement, _isDead = false;
    private readonly float _topSpeed = 16f, _timeToTopSpeed = .25f;
    private PlayerHealth _playerHealth;
    private BaseAbility _ability1, _ability2;
    private Dictionary<AbilityType, BaseAbility> _abilityMap = new Dictionary<AbilityType, BaseAbility>();
    public UnityEvent triggerScreenShake = new UnityEvent();
    [SerializeField] private AbilityType _ability1Type = AbilityType.Dodge;
    [SerializeField] private AbilityType _ability2Type = AbilityType.Scratch;
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private Animator _spriteAnimator;
    [SerializeField] private AudioClip _playerDeathClip;

    private void Awake()
    {
        BaseAbility[] abilities = GetComponentsInChildren<BaseAbility>();
        foreach (BaseAbility ability in abilities)
        {
            _abilityMap.Add(ability.AbilityType, ability);
        }
    }

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerHealth.TriggerDeath.AddListener(() => { PlayerDeath(); });
        var currentAbilities = DataManager.Instance.GetAbilities();
        _ability1Type = currentAbilities[0];
        _ability2Type = currentAbilities[1];
        SetAbilities();
    }

    public void SetAbilities(AbilityType[] abilities)
    {
        _ability1Type = abilities[0];
        _ability2Type = abilities[1];
    }

    public void PerformDodge()
    {
        _isDodging = true;
        IsInvincible = true;
        _playerRB.AddForce(CurrentDirection * _topSpeed, ForceMode2D.Impulse);
    }

    public void EndDodge()
    {
        _isDodging = false;
        IsInvincible = false;
    }

    public void HaltMovement()
    {
        _noMovement = true;
        _horizontalInput = 0;
        _verticalInput = 0;
    }

    public void RestoreMovement()
    {
        _noMovement = false;
    }

    public void TakePushback(float force, Vector3 direction)
    {
        _playerRB.AddForce(direction * force, ForceMode2D.Impulse);
        triggerScreenShake.Invoke();
        TimeManager.Instance.DoSlowmotion(.05f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Ability1"))
            _ability1Ended = true;

        if (Input.GetButtonUp("Ability2"))
            _ability2Ended = true;

        if (_isDodging || _noMovement || _isDead || TimeManager.Instance.IsPaused)
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
        if (_isDead) return;

        Vector3 targetDirection = new Vector3(_horizontalInput, _verticalInput, 0).normalized;
        if (_isDodging || _noMovement)
            targetDirection = CurrentDirection;

        Move(targetDirection);

        if (!_noMovement && _ability1Pressed)
            ActivateAbility1();

        if (!_noMovement && _ability2Pressed)
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
        var stopMovement = _noMovement;
        if (targetDirection.magnitude > 0)
        {
            CurrentDirection = targetDirection;
        }
        else
        {
            stopMovement = true;
        }

        if (!stopMovement)
        {
            _playerRB.drag = 0;
            Vector3 targetVelocity = targetDirection.normalized * _topSpeed;
            Vector2 diffVelocity = new Vector2(targetVelocity.x - _playerRB.velocity.x, targetVelocity.y - _playerRB.velocity.y);
            _playerRB.AddForce(diffVelocity / _timeToTopSpeed);
        }
        else
        {
            _playerRB.drag = _topSpeed / _timeToTopSpeed;
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

    private void PlayerDeath()
    {
        if (_isDead)
            return;

        _isDead = true;
        _spriteAnimator.SetTrigger("Death");
        SoundManager.Instance.PlaySound(_playerDeathClip, transform.position);
        GameManager.Instance.EndRun();
    }
}
