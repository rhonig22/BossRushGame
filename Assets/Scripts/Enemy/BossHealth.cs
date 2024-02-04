using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    public int Health { get; private set; } = 0;
    public UnityEvent TriggerDeath = new UnityEvent();
    private int _initialHealth = 350;
    private BaseBossController _bossController;
    [SerializeField] private HealthBar _healthBar;

    // Start is called before the first frame update
    void Awake()
    {
        SetMaxHealth(_initialHealth);
    }

    public void SetMaxHealth(int health)
    {
        Health = health;
        if (_healthBar != null)
            _healthBar.SetInitialVal(Health);
        _bossController = GetComponent<BaseBossController>();
    }

    public void TakeDamage(int damage)
    {
        if (_bossController.IsInvincible)
            return;

        Health -= damage;
        if (_healthBar != null)
            _healthBar.SetNewVal(Health);
        _bossController.Takehit();
        if (Health <= 0)
            TriggerDeath.Invoke();
    }
}
