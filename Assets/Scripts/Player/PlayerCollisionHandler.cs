using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerHealth _playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Enemy"))
        {
            BaseBossController boss = collision.gameObject.GetComponent<BaseBossController>();
            _playerHealth.TakeDamage(boss.DoDamage());
        }
    }
}
