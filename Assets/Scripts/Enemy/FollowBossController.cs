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
    }

    protected override void Move()
    {
        base.Move();
        if (_pauseMovement)
            return;

        transform.position = Vector3.MoveTowards(_rb.position, _player.position, CurrentSpeed * Time.fixedDeltaTime);
        _rb.velocity = Vector2.zero;
    }

    public override void Takehit()
    {
        if (!_enablePause)
            return;

        _pauseMovement = true;
        _spriteAnimator.SetBool("IsPaused", true);
        StartCoroutine(EndPause());
    }
}
