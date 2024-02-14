using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private PlayerController _playerController;
    [SerializeField] private AudioClip _hitSound;

    // Start is called before the first frame update
    void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_playerController.IsInvincible)
            return;

        if (collision.collider != null && collision.collider.CompareTag("Enemy"))
        {
            BaseBossController boss = collision.gameObject.GetComponent<BaseBossController>();
            _playerHealth.TakeDamage(boss.DoDamage());
            _playerController.TakePushback(boss.GetPushbackForce(), collision.GetContact(0).normal);
            SoundManager.Instance.PlaySound(_hitSound, transform.position);
        }
    }
}
