using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BearBossDifficulty
{
    public int Health;
    public int BaseDamage;
    public int RockCount;
    public int HiveCount;
    public float TriggerAttackChance;
    public float IdleLength;
    public Dictionary<BearAttackType, int> AttackChance = new Dictionary<BearAttackType, int>();

    public BearBossDifficulty(int health, int baseDamage, float triggerAttackChance, float idleLength, int rocks, int hives, Dictionary<BearAttackType, int> attachChance)
    {
        Health = health;
        BaseDamage = baseDamage;
        TriggerAttackChance = triggerAttackChance;
        IdleLength = idleLength;
        AttackChance = attachChance;
        RockCount = rocks;
        HiveCount = hives;
    }
}
