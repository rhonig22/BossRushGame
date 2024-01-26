using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerData()
    {
        Name = string.Empty;
        TimePassed = 0;
        BossesDefeated = 0;
        DamageDealt = 0;
        Health = 0;
        MaxHealth = 20;
        BaseDamage = 20;
    }

    public string Name;
    public float TimePassed;
    public int BossesDefeated;
    public int DamageDealt;
    public int Health;
    public int MaxHealth;
    public int BaseDamage;
}
