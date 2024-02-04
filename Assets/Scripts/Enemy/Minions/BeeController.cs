using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : FollowBossController
{
    protected int _beeHealth = 20;
    public bool IsAngry = false;
    private bool _beeStarted = false;
    [SerializeField] private AudioClip _beeStart;
    [SerializeField] private AudioClip _beeDie;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetComponent<BossHealth>().SetMaxHealth(_beeHealth);
        CurrentSpeed = 2f;
        _damage = 10;
    }

    protected override void Move()
    {
        if(IsAngry)
        {
            if (!_beeStarted)
            {
                SoundManager.Instance.PlaySound(_beeStart, transform.position);
                _beeStarted = true;
            }

            base.Move();
        }
    }

    protected override void EnemyDeath()
    {
        if (_isDying)
            return;

        _isDying = true;
        _pauseMovement = true;
        _collider.enabled = false;
        SoundManager.Instance.PlaySound(_beeDie, transform.position);
        _spriteAnimator.SetTrigger("death");
    }
}
