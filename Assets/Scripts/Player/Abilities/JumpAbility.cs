using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAbility : BaseAbility
{
    public static readonly float DodgeTime = .25f;
    private readonly string[] _enemyMask = new string[] { "Enemy" };
    private readonly string[] _nothingMask = new string[] { "Nothing" };
    private Collider2D _collider;
    private Rigidbody2D _rb;
    [SerializeField] private GameObject _mouse;
    [SerializeField] private GameObject _shadow;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        _shadow.SetActive(true);
        _mouse.transform.localScale = Vector3.zero;
        _collider.excludeLayers = LayerMask.GetMask(_enemyMask);
        _playerController.PerformJump();
    }

    public override void EndAbility()
    {
        base.EndAbility();
        _shadow.SetActive(false);
        _mouse.transform.localScale = Vector3.one;
        _collider.excludeLayers = LayerMask.GetMask(_nothingMask);
        _playerController.EndJump();
    }
}
