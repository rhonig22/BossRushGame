using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health { 
        get { return _health; }
        private set
        {
            _health = (int)Mathf.Clamp(value, 0, _maxHealth); ;
            DataManager.Instance.SetHealth(_health);
            if (_health == 0)
            {
                GameManager.Instance.EndRun();
            }
        }
    }

    private int _health = 0;
    private int _maxHealth = 0;
    [SerializeField] private HealthBar _healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _maxHealth = DataManager.Instance.GetMaxHealth();
        Health = DataManager.Instance.GetHealth();
        _healthBar.SetInitialVal(_maxHealth);
        _healthBar.SetNewVal(Health);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthBar.SetNewVal(Health);
    }

    public void AddHealth(float percent)
    {
        Health += Mathf.FloorToInt(percent * _maxHealth);
        _healthBar.SetNewVal(Health);
    }
}
