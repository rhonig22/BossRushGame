using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrogBossController : BaseBossController
{
    private FrogAttackType _currentAttack = FrogAttackType.Idle;
    private Vector2 _playerLocation = Vector2.zero;
    private Vector2 _startJumpLocation, _currentLocation;
    private bool _isDead = false;
    private int _bubbleStormCount = 5;
    private readonly float _bubbleDelay = .4f;
    private readonly float _jumpSpeed = 12f;
    [SerializeField] private GameObject _bubble;
    [SerializeField] private GameObject _bee;
    [SerializeField] private AudioClip _winceSound;
    [SerializeField] private AudioClip _roarSound;
    [SerializeField] private AudioClip _littleSpitSound;
    [SerializeField] private AudioClip _bigSpitSound;
    [SerializeField] private AudioClip _landingSound;
    public float AttackWaitPeriod { get; private set; } = 1.5f;
    public float AttackTriggerChance { get; private set; } = .4f;
    public Dictionary<FrogAttackType, int> AttackChance = new Dictionary<FrogAttackType, int>()
    {
        { FrogAttackType.Proximity, 25 },
        { FrogAttackType.Bubble, 50 },
        { FrogAttackType.BubbleStorm, 10 },
        { FrogAttackType.Bees, 15 },
        { FrogAttackType.Babies, 00 },
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        var difficulty = DataManager.Instance.FrogBossDifficulty;
        GetComponent<BossHealth>().SetMaxHealth(difficulty.Health);
        _damage = difficulty.BaseDamage;
        AttackChance = difficulty.AttackChance;
        AttackTriggerChance = difficulty.TriggerAttackChance;
        AttackWaitPeriod = difficulty.IdleLength;
        _bubbleStormCount = difficulty.BubbleStormCount;
    }

    protected override void Move()
    {
        base.Move();
        _rb.velocity = Vector2.zero;

        transform.position = Vector3.MoveTowards(_rb.position, _playerLocation, CurrentSpeed * Time.fixedDeltaTime);
        
        if(_currentAttack == FrogAttackType.Proximity)
        {
            float jumpProgress = (_startJumpLocation - _rb.position).magnitude / (_startJumpLocation - _playerLocation).magnitude;
            _bossAttackAnimator.SetFloat("JumpProgress", jumpProgress);
        }
    }

    public void PlayWinceSound()
    {
        SoundManager.Instance.PlaySound(_winceSound, transform.position);
    }

    public void PlayRoarSound()
    {
        SoundManager.Instance.PlaySound(_roarSound, transform.position);
    }

    public void PlayLittleSpitSound()
    {
        SoundManager.Instance.PlaySound(_littleSpitSound, transform.position);
    }

    public void PlayBigSpitSound()
    {
        SoundManager.Instance.PlaySound(_bigSpitSound, transform.position);
    }

    public void PlayLandingSound()
    {
        SoundManager.Instance.PlaySound(_landingSound, transform.position);
    }

    public override int DoDamage()
    {
        if (_isDead)
            return 0;

        switch (_currentAttack)
        {
            case FrogAttackType.Proximity:
                return _damage * 3;
            default:
                return _damage;
        }
    }

    public override float GetPushbackTime()
    {
        switch (_currentAttack)
        {
            case FrogAttackType.Proximity:
                return _pushbacktime * 3;
            default:
                return _pushbacktime;
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
        _bossAttackAnimator.SetTrigger(FrogAttackType.Proximity.ToString());
    }

    public void ReturnToIdle()
    {
        _currentAttack = FrogAttackType.Idle;
        _enablePause = true;
        IsInvincible = false;
        CurrentSpeed = 0;
        _bossAttackAnimator.SetTrigger("Idle");
    }

    public void StartAttack(FrogAttackType attackType)
    {
        _currentAttack = attackType;
        _enablePause = false;
        switch (attackType)
        {
            case FrogAttackType.Proximity:
                JumpAttack();
                break;
            case FrogAttackType.Bubble:
                BubbleAttack();
                break;
            case FrogAttackType.BubbleStorm:
                BubbleStormAttack();
                break;
            case FrogAttackType.Bees:
                BeesAttack();
                break;
            case FrogAttackType.Babies:
                BabiesAttack();
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
        SoundManager.Instance.PlaySound(_roarSound, transform.position);
        _bossAttackAnimator.SetTrigger("Death");
    }

    private void SetPlayerLocation()
    {
        _playerLocation = GameObject.FindWithTag("Player").transform.position;
    }

    public void JumpWindup()
    {
        PlayRoarSound();
        _enablePause = false;
    }

    private void JumpAttack()
    {
        SetPlayerLocation();
        _startJumpLocation = _rb.position;
        CurrentSpeed = _jumpSpeed;
        IsInvincible = true;
    }

    private void BubbleAttack()
    {
        SetPlayerLocation();
        PlayLittleSpitSound();
        CreateBubbles(1);
    }

    private void BubbleStormAttack()
    {
        SetPlayerLocation();
        PlayBigSpitSound();
        CreateBubbles(_bubbleStormCount);
    }
    private void BeesAttack()
    {
        SetPlayerLocation();
        PlayLittleSpitSound();
        CreateBee();
    }

    private void BabiesAttack()
    {
        SetPlayerLocation();
    }

    private void CreateBubbles(int count)
    {
        var playerDistance = (Vector3)_playerLocation - transform.position;
        Vector3 spawnPosition = transform.position;
        Vector3 offset = Vector3.zero;
        if (Mathf.Abs(playerDistance.x) > Mathf.Abs(playerDistance.y))
        {
            spawnPosition += new Vector3(playerDistance.x, 0, 0).normalized;
            offset = new Vector3(0, .6f, 0);
        }
        else
        {
            spawnPosition += new Vector3(0, playerDistance.y, 0).normalized;
            offset = new Vector3(.6f, 0, 0);
        }

        var flip = -1;
        List<BubbleController> bubbles = new List<BubbleController>();
        for (int i = 0; i < count; i++)
        {
            var bubble = Instantiate(_bubble);
            bubble.transform.position = spawnPosition + (offset * (int)((i + 1) / 2) * flip);
            flip *= -1;
            bubbles.Add(bubble.GetComponent<BubbleController>());
        }

        for (int i = 0; i < count; i++)
        {
            bubbles[i].SendBubbleWithDelay(_bubbleDelay * (i + 1), _playerLocation);
        }
    }

    private void CreateBee(){
        Vector3 spawnPosition = transform.position;
        var bee = Instantiate(_bee);
        bee.transform.position = spawnPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && _currentAttack == FrogAttackType.Proximity && collision.collider.CompareTag("Wall"))
        {
            _bossAttackAnimator.SetFloat("JumpProgress", 1f);
        }
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