using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class JumpAbility : BaseAbility
{
    private readonly float _jumpTime = .25f, _initialShadowSpeed = 6f, _aoeRadius = 2f, _aoeDamageMultiplier = 1.5f, _aoeTime = .1f, _initialDistance = 1.5f;
    private readonly string[] _enemyMask = new string[] { "Enemy" };
    private readonly string[] _wallMask = new string[] { "Wall" };
    private readonly string[] _nothingMask = new string[] { "Nothing" };
    private Collider2D _collider;
    private bool _isJumping = false, _isShadowMoving = false;
    private float _shadowSpeed = 5f, _jumpSpeed = 5f;
    private Vector3 _endPosition = Vector3.zero;
    [SerializeField] private GameObject _mouse;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private AudioClip _startJumpSound;
    [SerializeField] private AudioClip _endJumpSound;
    [SerializeField] private ParticleSystem _aoeParticles;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (_isShadowMoving)
        {
            var direction = _playerController.CurrentDirection;
            var distance = _shadowSpeed * Time.fixedDeltaTime;
            RaycastHit2D hit = Physics2D.CircleCast(_shadow.transform.position, distance, direction, distance, LayerMask.GetMask(_wallMask));
            if (hit.collider != null)
                return;

            _shadow.transform.localPosition += direction * distance;
            _shadowSpeed += _shadowSpeed * Time.fixedDeltaTime;
        }
        else if (_isJumping)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPosition, _jumpSpeed * Time.fixedDeltaTime);
            if (transform.position == _endPosition)
                EndJump();
        }
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        _shadow.SetActive(true);
        _isShadowMoving = true;
        _shadowSpeed = _initialShadowSpeed;
        _shadow.transform.localPosition += _playerController.CurrentDirection * _initialDistance;
        _playerController.HaltMovement();
        SoundManager.Instance.PlaySound(_startJumpSound, transform.position);
    }

    public override void EndAbility()
    {
        base.EndAbility();
        _shadow.SetActive(false);
        _isShadowMoving = false;
        StartJump();
    }


    private void StartJump()
    {
        _isJumping = true;
        _collider.excludeLayers = LayerMask.GetMask(_enemyMask);
        _endPosition = _shadow.transform.position;
        _jumpSpeed = (_endPosition - transform.position).magnitude / _jumpTime;
        SoundManager.Instance.PlaySound(_endJumpSound, transform.position);
    }

    private void EndJump()
    {
        _isJumping = false;
        _collider.excludeLayers = LayerMask.GetMask(_nothingMask);
        _playerController.RestoreMovement();
        _shadow.transform.localPosition = Vector3.zero;
        AreaOfEffectAttack();
    }

    private void AreaOfEffectAttack()
    {
        _aoeParticles.Play();
        _playerController.triggerScreenShake.Invoke();  
        _playerController.EnableInvincibility(true);
        var colliders = Physics2D.OverlapCircleAll(transform.position, _aoeRadius, LayerMask.GetMask(_enemyMask));

        StartCoroutine(EndAreaOfEffect(colliders));
    }

    private IEnumerator EndAreaOfEffect(Collider2D[] colliders)
    {
        yield return new WaitForSeconds(_aoeTime);
        _playerController.EnableInvincibility(false);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<BossHealth>().TakeDamage(DataManager.Instance.GetDamage(_aoeDamageMultiplier));
                var collisionPoint = collider.ClosestPoint(transform.position);
                collider.GetComponent<BaseBossController>().SprayParticles(collisionPoint);
            }
        }
    }
}
