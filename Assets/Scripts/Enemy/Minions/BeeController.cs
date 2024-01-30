using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : FollowBossController
{
    protected int _beeHealth = 20;
    public bool IsAngry = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetComponent<BossHealth>().SetMaxHealth(_beeHealth);
        CurrentSpeed = 2f;
    }

    protected override void Move()
    {
        if(IsAngry)
        {
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
        _spriteAnimator.SetTrigger("death");
    }
}
