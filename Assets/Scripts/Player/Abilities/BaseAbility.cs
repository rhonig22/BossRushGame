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
}

public enum AbilityType
{
    Scratch = 0,
    Dodge = 1,
}