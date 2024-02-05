using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int CurrentHighScore;
    public float TimePassed;
    public int BossesDefeated;
    public int DamageDealt;
    public int Health;
    public int MaxHealth;
    public int BaseDamage;
    public AbilityType[] Abilities = new AbilityType[2];

    public PlayerData(AbilityType ability1 = AbilityType.Dodge, AbilityType ability2 = AbilityType.Scratch)
    {
        TimePassed = 0;
        BossesDefeated = 0;
        DamageDealt = 0;
        Health = 20;
        MaxHealth = 20;
        BaseDamage = 20;
        Abilities[0] = ability1;
        Abilities[1] = ability2;
    }
}
