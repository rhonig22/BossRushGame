using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string Name;
    public float TimePassed;
    public int BossesDefeated;
    public int DamageDealt;
    public int Health;
    public int MaxHealth;
    public int BaseDamage;
    public AbilityType[] Abilities = new AbilityType[2];

    public PlayerData()
    {
        Name = string.Empty;
        TimePassed = 0;
        BossesDefeated = 0;
        DamageDealt = 0;
        Health = 20;
        MaxHealth = 20;
        BaseDamage = 20;
        Abilities[0] = AbilityType.Dodge;
        Abilities[1] = AbilityType.Scratch;
    }
}
