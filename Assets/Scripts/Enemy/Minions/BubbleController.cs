using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : FollowBossController
{
    protected int _bubbleHealth = 20;
    private Vector3 _startPosition, _endPosition;
    private float _desiredDuration = 1.2f;
    private float _elapsedTime, _percentageComplete;
    private bool _startMovement = false;
    [SerializeField] private AnimationCurve _curve;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetComponent<BossHealth>().SetMaxHealth(_bubbleHealth);
        _startPosition = transform.position;
    }

    protected override void Move()
    {
        if (_startMovement)
        {
            _elapsedTime += Time.deltaTime;
            _percentageComplete = _elapsedTime / _desiredDuration;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, _curve.Evaluate(_percentageComplete));
        }
    }

    public IEnumerator SendBubble(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _endPosition = _player.position;
        _startMovement = true;
    }

    // Remove bubble if the player touches it (will work even if player has immunity frames)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Player"))
        {
            EnemyDeath();
        }
    }

    protected override void EnemyDeath()
    {
        _pauseMovement = true;
        _collider.enabled = false;
        _spriteAnimator.SetTrigger("death");
    }
}
