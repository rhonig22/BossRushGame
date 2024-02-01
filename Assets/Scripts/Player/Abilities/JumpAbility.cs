using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAbility : BaseAbility
{
    private readonly float _jumpTime = .25f;
    private readonly float _initialShadowSpeed = 6f;
    private readonly string[] _enemyMask = new string[] { "Enemy" };
    private readonly string[] _nothingMask = new string[] { "Nothing" };
    private Collider2D _collider;
    private bool _isJumping = false, _isShadowMoving = false;
    private float _shadowSpeed = 5f, _jumpSpeed = 5f;
    private Vector3 _endPosition = Vector3.zero;
    [SerializeField] private GameObject _mouse;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private AudioClip _startJumpSound;
    [SerializeField] private AudioClip _endJumpSound;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (_isShadowMoving)
        {
            var direction = _playerController.CurrentDirection;
            _shadow.transform.localPosition += direction * _shadowSpeed * Time.fixedDeltaTime;
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
    }
}
