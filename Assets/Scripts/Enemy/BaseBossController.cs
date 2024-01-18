using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossController : MonoBehaviour
{
    private Transform _player;
    private float _speed = 4f;
    private Rigidbody2D _rb;
    private readonly int _damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(_rb.position, _player.position, _speed * Time.fixedDeltaTime);
        _rb.velocity = Vector2.zero;
    }

    public int DoDamage()
    {
        return _damage;
    }
}
