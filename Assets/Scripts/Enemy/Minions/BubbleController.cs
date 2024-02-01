using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubbleController : FollowBossController
{
    public UnityEvent BubbleSent { get; private set; } = new UnityEvent();
    protected int _bubbleHealth = 20;
    private Vector3 _startPosition, _endPosition;
    private float _desiredDuration = 1.2f;
    private float _elapsedTime, _percentageComplete;
    private bool _startMovement = false;
    private IEnumerator _bubbleCoroutine = null;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private string _objectToSeek = "Player";
    [SerializeField] private AudioClip _bubbleStart;
    [SerializeField] private AudioClip _bubblePop;

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

    public IEnumerator SendBubble(float waitTime, Vector3 endPosition)
    {
        yield return new WaitForSeconds(waitTime);
        if (_objectToSeek == "Player")
            endPosition = _player.position;
        _endPosition = endPosition;
        _startMovement = true;
        SoundManager.Instance.PlaySound(_bubbleStart, transform.position);
        BubbleSent.Invoke();
    }

    public void SetBubbleCoroutine(IEnumerator bubbleCoroutine)
    {
        _bubbleCoroutine = bubbleCoroutine;
    }

    private Vector3 GetNearestEnemyPosition()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            return Vector3.zero;

        Vector3 closestEnemyPosition = enemies[0].transform.position;
        foreach (var enemy in enemies)
        {
            if ((enemy.transform.position - transform.position).magnitude > (closestEnemyPosition - transform.position).magnitude)
                closestEnemyPosition = enemy.transform.position;
        }

        return closestEnemyPosition;
    }

    // Remove bubble if the player touches it (will work even if player has immunity frames)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag(_objectToSeek))
        {
            if (_objectToSeek == "Enemy")
            {
                collision.collider.GetComponent<BossHealth>().TakeDamage(DoDamage());
            }

            EnemyDeath();
        }
    }

    protected override void EnemyDeath()
    {
        if (_isDying)
            return;

        _isDying = true;
        _pauseMovement = true;
        _collider.enabled = false;
        SoundManager.Instance.PlaySound(_bubblePop, transform.position);
        if (_bubbleCoroutine != null)
            StopCoroutine(_bubbleCoroutine);

        _spriteAnimator.SetTrigger("death");
    }
}
