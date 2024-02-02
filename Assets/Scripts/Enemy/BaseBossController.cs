using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BaseBossController : MonoBehaviour
{
    protected readonly float _pushbacktime = .25f, _pauseTime = .5f;
    protected Transform _player;
    protected Rigidbody2D _rb;
    protected Collider2D _collider;
    protected BossHealth _health;
    protected bool _enablePause = false, _pauseMovement = false, _isDying = false;
    [SerializeField] protected int _damage = 5;
    [SerializeField] protected Animator _spriteAnimator;
    [SerializeField] protected Animator _bossAttackAnimator;
    [SerializeField] private AudioClip _damagedSound;
    public float CurrentSpeed { get; protected set; } = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _health = GetComponent<BossHealth>();
        _health.TriggerDeath.AddListener(() => EnemyDeath());
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (_pauseMovement)
            return;

        Move();
    }

    protected virtual void Move()
    {

    }

    public virtual int DoDamage()
    {
        return _damage;
    }

    public virtual float GetPushbackTime()
    {
        return _pushbacktime;
    }

    public virtual void Takehit()
    {
        if (!_enablePause)
            return;

        _pauseMovement = true;
        _spriteAnimator.SetBool("IsPaused", true);
        SoundManager.Instance.PlaySound(_damagedSound, transform.position);
        StartCoroutine(EndPause());
    }

    protected IEnumerator EndPause()
    {
        yield return new WaitForSeconds(_pauseTime);
        _pauseMovement = false;
        _spriteAnimator.SetBool("IsPaused", false);
    }

    protected virtual void EnemyDeath()
    {
        DestroySelf();
    }

    public void DestroySelf()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
