using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleContoller : FollowBossController
{
    protected int _bubbleHealth = 20;
    protected float _initialSpeed = 4f, _distToPlayer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        CurrentSpeed = _initialSpeed;
        _enablePause = true;
        GetComponent<BossHealth>().SetMaxHealth(_bubbleHealth);
        _distToPlayer = (_player.position - transform.position).magnitude;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CurrentSpeed = _initialSpeed * ((_player.position - transform.position).magnitude / _distToPlayer);
    }
}
