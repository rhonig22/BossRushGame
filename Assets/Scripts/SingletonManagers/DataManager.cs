using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private PlayerData _playerData = new PlayerData();
    public float TimePassed { get; private set; } = 0f;
    public bool IsTimeStarted { get; private set; } = false;
    public bool ShouldPauseAtStart { get; private set; } = true;
    [SerializeField] private AbilityType _initialAbility1 = AbilityType.Dodge;
    [SerializeField] private AbilityType _initialAbility2 = AbilityType.Scratch;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResetData();
    }

    private void Update()
    {
        if (IsTimeStarted)
        {
            TimePassed += Time.deltaTime;
        }
    }
    public void StartTimer()
    {
        IsTimeStarted = true;
    }
    public void PauseTimer()
    {
        IsTimeStarted = false;
        SetTime(TimePassed);
    }

    public void ResetData()
    {
        _playerData = new PlayerData(_initialAbility1, _initialAbility2);
        TimePassed = 0f;
    }

    public void SetTime(float time)
    {
        _playerData.TimePassed = time;
    }

    public string GetTime()
    {
        return TimeSpan.FromSeconds((double)_playerData.TimePassed).ToString(@"mm\:ss");
    }

    public void SetHealth(int health)
    {
        _playerData.Health = health;
    }

    public int GetHealth() { return _playerData.Health; }

    public int GetMaxHealth() { return _playerData.MaxHealth; }

    public void SetName(string name)
    {
        _playerData.Name = name;
    }

    public string GetName() { return _playerData.Name; }

    public void AddDamageDealt(int damage)
    {
        _playerData.DamageDealt += damage;
    }

    public int GetDamageDealt() { return _playerData.DamageDealt; }

    public void AddBossDefeated()
    {
        _playerData.BossesDefeated += 1;
    }

    public int GetBossesDefeated() { return _playerData.BossesDefeated; }

    public int GetDamage(int multiplier)
    {
        var damage = _playerData.BaseDamage * multiplier;
        AddDamageDealt(damage);
        return damage;
    }

    public AbilityType[] GetAbilities() { return _playerData.Abilities; }

    public void SetAbilities(AbilityType ability1, AbilityType ability2)
    {
        _playerData.Abilities[0] = ability1;
        _playerData.Abilities[1] = ability2;
    }

    public void InitialPauseComplete()
    {
        ShouldPauseAtStart = false;
    }
}
