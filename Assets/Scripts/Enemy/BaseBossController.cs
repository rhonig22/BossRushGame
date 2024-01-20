using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BaseBossController : MonoBehaviour
{
    private Transform _player;
    private float _speed = 4f;
    private Rigidbody2D _rb;
    private bool _pauseMovement = false;
    private readonly int _damage = 5;
    private readonly float _pushbacktime = .25f, _pauseTime = .5f;
    [SerializeField] private Animator _spriteAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_pauseMovement)
            return;

        transform.position = Vector3.MoveTowards(_rb.position, _player.position, _speed * Time.fixedDeltaTime);
        _rb.velocity = Vector2.zero;
    }

    public virtual int DoDamage()
    {
        return _damage;
    }

    public virtual float GetPushbackTime()
    {
        return _pushbacktime;
    }

    public virtual void Takehit()
    {
        _pauseMovement = true;
        _spriteAnimator.SetBool("IsPaused", true);
        StartCoroutine(EndPause());
    }

    private IEnumerator EndPause()
    {
        yield return new WaitForSeconds(_pauseTime);
        _pauseMovement = false;
        _spriteAnimator.SetBool("IsPaused", false);
    }
}
