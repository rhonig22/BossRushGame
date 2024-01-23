using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BaseBossController : MonoBehaviour
{
    protected float _speed = 0;
    protected readonly int _damage = 5;
    protected readonly float _pushbacktime = .25f, _pauseTime = .5f;
    protected Transform _player;
    protected Rigidbody2D _rb;
    protected bool _enablePause = false;
    protected bool _pauseMovement = false;
    [SerializeField] protected Animator _spriteAnimator;
    [SerializeField] protected Animator _bossAttackAnimator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
        StartCoroutine(EndPause());
    }

    protected IEnumerator EndPause()
    {
        yield return new WaitForSeconds(_pauseTime);
        _pauseMovement = false;
        _spriteAnimator.SetBool("IsPaused", false);
    }
}
