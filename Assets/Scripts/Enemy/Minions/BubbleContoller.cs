using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleContoller : FollowBossController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _speed = 4f;
        _enablePause = true;
    }
}
