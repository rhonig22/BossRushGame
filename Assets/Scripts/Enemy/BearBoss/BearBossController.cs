using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearBossController : BaseBossController
{
    private BearAttackType _currentAttack = BearAttackType.Idle;
    private Vector2 _startRushLocation, _endLocation, _playerLocation = Vector2.zero;
    private bool _isDead = false;
    public float AttackWaitPeriod { get; private set; } = 1.5f;
    public float AttackTriggerChance { get; private set; } = .4f;
    private readonly float _rushSpeed = 10f, _rushPastDistance = 1.5f;
    public bool IsStunned { get; protected set; } = false;
    public Dictionary<BearAttackType, int> AttackChance = new Dictionary<BearAttackType, int>()
    {
        { BearAttackType.Rush, 40 },
        { BearAttackType.Maul, 60 },
        { BearAttackType.Rocks, 0 },
        { BearAttackType.Hive, 0 },
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        var difficulty = DataManager.Instance.BearBossDifficulty;
        GetComponent<BossHealth>().SetMaxHealth(difficulty.Health);
        _damage = difficulty.BaseDamage;
        AttackChance = difficulty.AttackChance;
        AttackTriggerChance = difficulty.TriggerAttackChance;
        AttackWaitPeriod = difficulty.IdleLength;
    }

    protected override void Move()
    {
        base.Move();
        _rb.velocity = Vector2.zero;

        transform.position = Vector3.MoveTowards(_rb.position, _endLocation, CurrentSpeed * Time.fixedDeltaTime);

        if (_currentAttack == BearAttackType.Rush && !IsStunned)
        {
            float rushProgress = (_startRushLocation - _rb.position).magnitude / (_startRushLocation - _endLocation).magnitude;
            if (rushProgress > .95f)
                rushProgress = 1f;

            _bossAttackAnimator.SetFloat("RushProgress", rushProgress);
        }
    }

    public override int DoDamage()
    {
        if (_isDead)
            return 0;

        switch (_currentAttack)
        {
            case BearAttackType.Rush:
                return _damage * 3;
            default:
                return _damage;
        }
    }

    public override float GetPushbackForce()
    {
        switch (_currentAttack)
        {
            case BearAttackType.Rush:
                return _pushbackForce * 2;
            default:
                return _pushbackForce;
        }
    }

    public void DetermineNextAttack()
    {
        Anger = 0;
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

    public void AngerAttack()
    {
        Anger = 0;
        _bossAttackAnimator.SetTrigger(BearAttackType.Maul.ToString());
    }

    public void ReturnToIdle()
    {
        _currentAttack = BearAttackType.Idle;
        _enablePause = true;
        IsInvincible = false;
        IsStunned = false;
        CurrentSpeed = 0;
        _bossAttackAnimator.SetTrigger("Idle");
    }


    public void StartAttack(BearAttackType attackType)
    {
        _currentAttack = attackType;
        _enablePause = false;
        switch (attackType)
        {
            case BearAttackType.Rush:
                RushAttack();
                break;
            case BearAttackType.Maul:
                MaulAttack();
                break;
            case BearAttackType.Rocks:
                RocksAttack();
                break;
            case BearAttackType.Hive:
                HiveAttack();
                break;
            default:
                break;
        }
    }

    protected override void EnemyDeath()
    {
        if (_isDead)
            return;

        _isDead = true;
        GameManager.Instance.DefeatedBoss();
        _bossAttackAnimator.SetTrigger("Death");
    }

    private void SetPlayerLocation()
    {
        _playerLocation = GameObject.FindWithTag("Player").transform.position;
    }

    public void RushWindup()
    {
    }

    private void RushAttack()
    {
        SetPlayerLocation();
        _startRushLocation = _rb.position;
        var direction = (_playerLocation - _startRushLocation).normalized * _rushPastDistance;
        _endLocation = _playerLocation + direction;
        CurrentSpeed = _rushSpeed;
        IsInvincible = true;
    }

    private void MaulAttack()
    {
        SetPlayerLocation();
        // TODO: implement attack
    }

    private void RocksAttack()
    {
        SetPlayerLocation();
        // TODO: implement attack
    }
    private void HiveAttack()
    {
        SetPlayerLocation();
        // TODO: implement attack
    }

    private void Stunned()
    {
        IsInvincible = false;
        IsStunned = true;
        CurrentSpeed = 0;
        _bossAttackAnimator.SetTrigger("Stunned");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && _currentAttack == BearAttackType.Rush && collision.collider.CompareTag("Wall"))
        {
            Stunned();
        }
    }
}

public enum BearAttackType
{
    Idle = 0,
    Rush = 1,
    Maul = 2,
    Rocks = 3,
    Hive = 4,
}