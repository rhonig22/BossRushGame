using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    public AbilityType AbilityType;
    public string Name = string.Empty;
    protected Rigidbody2D _playerRB;
    protected PlayerController _playerController;

    private void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerController>();
    }

    public virtual void ActivateAbility()
    {

    }

    public virtual void EndAbility()
    {

    }
}

public class Ability
{
    public string Name = string.Empty;
    public AbilityType Type;
    public bool IsTaken = false;
    public Ability(string name, AbilityType type)
    {
        Name = name;
        Type = type;
    }
}

public enum AbilityType
{
    Scratch = 0,
    Dodge = 1,
    Bubble = 2,
    Jump = 3,
}