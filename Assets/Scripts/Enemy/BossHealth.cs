using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int Health { get; private set; } = 0;
    private int _initialHealth = 350;
    private BaseBossController _bossController;
    [SerializeField] private HealthBar _healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Health = _initialHealth;
        _healthBar.SetInitialVal(Health);
        _bossController = GetComponent<BaseBossController>();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthBar.SetNewVal(Health);
        _bossController.Takehit();
    }
}