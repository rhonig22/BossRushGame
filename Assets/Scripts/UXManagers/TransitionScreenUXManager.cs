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
    [SerializeField] private TextMeshProUGUI _ability1Name;
    [SerializeField] private TextMeshProUGUI _ability2Name;
    private bool _someCheeseSelected = false;
    private bool _rewardSelected = false;

    private void Start()
    {
        SetCurrentAbilities();
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
        var abilities = DataManager.Instance.GetAbilities();
        DataManager.Instance.SetAbilities(abilities[0], AbilityType.Bubble);
        SetCurrentAbilities();
    }

    public void Ability2Selected()
    {
        if (_rewardSelected)
            return;

        RewardSold();
        var abilities = DataManager.Instance.GetAbilities();
        DataManager.Instance.SetAbilities(AbilityType.Jump, abilities[1]);
        SetCurrentAbilities();
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

    private void SetCurrentAbilities()
    {
        var abilities = DataManager.Instance.GetAbilities();
        _ability1Name.text = abilities[0].ToString();
        _ability2Name.text = abilities[1].ToString();
    }
}
