using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : FollowBossController
{
    protected int _bubbleHealth = 20;
    private Vector3 _startPosition, _endPosition;
    private float _desiredDuration = 1.2f;
    private float _elapsedTime, _percentageComplete;
    [SerializeField] private AnimationCurve _curve;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _enablePause = true;
        GetComponent<BossHealth>().SetMaxHealth(_bubbleHealth);
        _startPosition = transform.position;
        _endPosition = _player.position;
    }

    protected override void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
        _percentageComplete = _elapsedTime / _desiredDuration;
        transform.position = Vector3.Lerp(_startPosition, _endPosition, _curve.Evaluate(_percentageComplete));
    }

    protected override void Move()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Player"))
        {
            GetComponent<BossHealth>().EnemyDeath();
        }
    }
}
