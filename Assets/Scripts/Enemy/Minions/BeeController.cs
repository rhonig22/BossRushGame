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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Player"))
        {

        }
    }

    protected override void EnemyDeath()
    {
        _pauseMovement = true;
        _collider.enabled = false;
        _spriteAnimator.SetTrigger("death");
    }
}
