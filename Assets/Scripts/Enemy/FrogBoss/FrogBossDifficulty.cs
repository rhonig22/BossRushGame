using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrogBossDifficulty
{
    public int Health;
    public int BaseDamage;
    public int BubbleStormCount;
    public float TriggerAttackChance;
    public float IdleLength;
    public Dictionary<FrogAttackType, int> AttackChance = new Dictionary<FrogAttackType, int>();

    public FrogBossDifficulty(int health, int baseDamage, float triggerAttackChance, float idleLength, int bubbleStorm, Dictionary<FrogAttackType, int> attachChance)
    {
        Health = health;
        BaseDamage = baseDamage;
        TriggerAttackChance = triggerAttackChance;
        IdleLength = idleLength;
        AttackChance = attachChance;
        BubbleStormCount = bubbleStorm;
    }
}
