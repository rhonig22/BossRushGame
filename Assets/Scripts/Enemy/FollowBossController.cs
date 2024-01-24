using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FollowBossController: BaseBossController
{
    protected override void Start()
    {
        base.Start();
        CurrentSpeed = 4f;
        _enablePause = true;
    }

    protected override void Move()
    {
        base.Move();
        transform.position = Vector3.MoveTowards(_rb.position, _player.position, CurrentSpeed * Time.fixedDeltaTime);
        _rb.velocity = Vector2.zero;
    }

    public override void Takehit()
    {
        _pauseMovement = true;
        _spriteAnimator.SetBool("IsPaused", true);
        StartCoroutine(EndPause());
    }
}
