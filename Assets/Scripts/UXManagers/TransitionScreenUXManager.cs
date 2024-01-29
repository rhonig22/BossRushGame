using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionScreenUXManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _soldSomeCheese;
    [SerializeField] private GameObject _soldAbility1;
    [SerializeField] private GameObject _soldAbility2;
    [SerializeField] private GameObject _soldHeckaCheese;
    private bool _someCheeseSelected = false;
    private bool _rewardSelected = false;

    private void Start()
    {
    }

    public void StartBoss()
    {
        GameManager.Instance.LoadNextBoss();
    }

    public void SomeCheeseSelected()
    {
        if (_someCheeseSelected)
            return;

        SomeCheeseSold();
        _playerHealth.AddHealth(.25f);
    }

    public void HeckaCheeseSelected()
    {
        if (_rewardSelected)
            return;

        RewardSold();
        _playerHealth.AddHealth(1f);
    }

    public void Ability1Selected()
    {
        if (_rewardSelected)
            return;

        RewardSold();
    }

    public void Ability2Selected()
    {
        if (_rewardSelected)
            return;

        RewardSold();
    }

    private void SomeCheeseSold()
    {
        _someCheeseSelected = true;
        _soldSomeCheese.SetActive(true);
    }

    private void RewardSold()
    {
        _rewardSelected = true;
        _soldAbility1.SetActive(true);
        _soldAbility2.SetActive(true);
        _soldHeckaCheese.SetActive(true);
    }
}
