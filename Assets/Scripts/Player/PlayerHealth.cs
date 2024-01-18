using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health { get; private set; } = 0;
    private int _initialHealth = 20;
    [SerializeField] private HealthBar _healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Health = _initialHealth;
        _healthBar.SetInitialVal(Health);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthBar.SetNewVal(Health);
    }
}
