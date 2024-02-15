using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreenUXManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Button _ability1Button;
    [SerializeField] private Button _ability2Button;
    [SerializeField] private Button _heckaCheeseButton;
    [SerializeField] private Button _someCheeseButton;
    [SerializeField] private GameObject _soldSomeCheese;
    [SerializeField] private GameObject _soldAbility1;
    [SerializeField] private GameObject _soldAbility2;
    [SerializeField] private GameObject _soldHeckaCheese;
    [SerializeField] private GameObject _freeTag;
    [SerializeField] private TextMeshProUGUI _ability1Name;
    [SerializeField] private TextMeshProUGUI _ability2Name;
    [SerializeField] private TextMeshProUGUI _reward1Name;
    [SerializeField] private TextMeshProUGUI _reward2Name;
    [SerializeField] private SelectPopupUXManager _selectPopup;
    [SerializeField] private Button _proceedButton;
    [SerializeField] private AudioClip _cheeseEatingSound;
    private bool _someCheeseSelected = false;
    private bool _rewardSelected = false;
    private Ability _reward1;
    private Ability _reward2;

    private void Start()
    {
        SetCurrentAbilities();
        SetCurrentRewards();
        _ability1Button.Select();
        _selectPopup.Finished.AddListener(() =>
        {
            SetCurrentAbilities();
            
            if (_someCheeseSelected)
            {
                _proceedButton.Select();
            }else{
                _someCheeseButton.Select();
            }
        });

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
        SoundManager.Instance.PlaySound(_cheeseEatingSound, transform.position);
        
        if (_rewardSelected)
        {
            _proceedButton.Select();
        }else{
            _heckaCheeseButton.Select();
        }
    }

    public void HeckaCheeseSelected()
    {
        if (_rewardSelected)
            return;

        RewardSold();
        _soldHeckaCheese.SetActive(true);
        _playerHealth.AddHealth(1f);
        SoundManager.Instance.PlaySound(_cheeseEatingSound, transform.position);

        if (_someCheeseSelected)
        {
            _proceedButton.Select();
        }else{
            _someCheeseButton.Select();
        }
    }

    public void Ability1Selected()
    {
        if (_rewardSelected)
            return;

        RewardSold();
        _soldAbility1.SetActive(true);
        _selectPopup.Open(DataManager.Instance.Rewards[0,0].Type, 0);
    }

    public void Ability2Selected()
    {
        if (_rewardSelected)
            return;

        RewardSold();
        _soldAbility2.SetActive(true);
        _selectPopup.Open(DataManager.Instance.Rewards[0, 1].Type, 1);
    }

    private void SomeCheeseSold()
    {
        _someCheeseSelected = true;
        _soldSomeCheese.SetActive(true);
        _someCheeseButton.interactable = false;
        _freeTag.GetComponent<Image>().enabled = false;
        if (!_rewardSelected)
        {
            var navigation = _heckaCheeseButton.navigation;
            navigation.selectOnDown = _proceedButton;
            _heckaCheeseButton.navigation = navigation;

            navigation = _proceedButton.navigation;
            navigation.selectOnUp = _heckaCheeseButton;
            _proceedButton.navigation = navigation;
        }
        else
        {
            var navigation = _proceedButton.navigation;
            navigation.selectOnUp = null;
            _proceedButton.navigation = navigation;
        }
    }

    private void RewardSold()
    {
        _rewardSelected = true;
        _ability1Button.interactable = false;
        _ability2Button.interactable = false;
        _heckaCheeseButton.interactable = false;
        if (!_someCheeseSelected)
        {
            var navigation = _someCheeseButton.navigation;
            navigation.selectOnUp = null;
            _someCheeseButton.navigation = navigation;
        }
        else
        {
            var navigation = _proceedButton.navigation;
            navigation.selectOnUp = null;
            _proceedButton.navigation = navigation;
        }
    }

    private void SetCurrentAbilities()
    {
        var abilities = DataManager.Instance.GetAbilities();
        _ability1Name.text = abilities[0].ToString();
        _ability2Name.text = abilities[1].ToString();
    }

    private void SetCurrentRewards()
    {
        _reward1 = DataManager.Instance.Rewards[0, 0];
        _reward2 = DataManager.Instance.Rewards[0, 1];
        _reward1Name.text = _reward1.Name == string.Empty ? _reward1.Type.ToString() : _reward1.Name;
        _reward2Name.text = _reward2.Name == string.Empty ? _reward2.Type.ToString() : _reward2.Name;
    }
}
