using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrogBossController : BaseBossController
{
    private FrogAttackType _currentAttack = FrogAttackType.Idle;
    private Vector2 _playerLocation = Vector2.zero;
    [SerializeField] private GameObject _bubble;
    public readonly Dictionary<FrogAttackType, int> AttackChance = new Dictionary<FrogAttackType, int>()
    {
        { FrogAttackType.Proximity, 40 },
        { FrogAttackType.Bubble, 25 },
        { FrogAttackType.BubbleStorm, 25 },
        { FrogAttackType.Bees, 10 },
        { FrogAttackType.Babies, 0 },
    };

    protected override void Move()
    {
        base.Move();
        _rb.velocity = Vector2.zero;

        transform.position = Vector3.MoveTowards(_rb.position, _playerLocation, _speed * Time.fixedDeltaTime);
    }

    public void DetermineNextAttack()
    {
        int roll = Random.Range(0, 100);
        int totalChance = 0;
        foreach (var attack in AttackChance.Keys)
        {
            totalChance += AttackChance[attack];
            if (roll < totalChance)
            {
                _bossAttackAnimator.SetTrigger(attack.ToString());
                break;
            }
        }
    }

    public void ReturnToIdle()
    {
        _currentAttack = FrogAttackType.Idle;
        _speed = 0;
        _bossAttackAnimator.SetTrigger("Idle");
    }

    public void StartAttack(FrogAttackType attackType)
    {
        _currentAttack = attackType;
        switch (attackType)
        {
            case FrogAttackType.Proximity:
                JumpAttack();
                break;
            case FrogAttackType.Bubble:
                BubbleAttack();
                break;
            default:
                break;
        }
    }

    private void JumpAttack()
    {
        _playerLocation = GameObject.FindWithTag("Player").transform.position;
        _speed = (_playerLocation - _rb.position).magnitude;
    }

    private void BubbleAttack()
    {
        var bubble = GameObject.Instantiate(_bubble);
        bubble.transform.position = transform.position + new Vector3(0, -1, 1);
    }
}

public enum FrogAttackType
{
    Idle = 0,
    Proximity = 1,
    Bubble = 2,
    BubbleStorm = 3,
    Bees = 4,
    Babies = 5,
}