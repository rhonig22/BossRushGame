using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectPopupUXManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectPopup;
    [SerializeField] private TextMeshProUGUI _newAbilityName;
    [SerializeField] private TextMeshProUGUI _ability1Name;
    [SerializeField] private TextMeshProUGUI _ability2Name;
    [SerializeField] private Button _selectButton;
    private AbilityType _newAbility;
    private AbilityType[] _abilities;
    private int _rewardNum;
    public UnityEvent Finished {  get; private set; } = new UnityEvent();

    public void Open(AbilityType newAbility, int rewardNum)
    {
        _newAbility = newAbility;
        _rewardNum = rewardNum;
        _abilities = DataManager.Instance.GetAbilities();
        _newAbilityName.text = newAbility.ToString() + "?";
        _ability1Name.text = _abilities[0].ToString();
        _ability2Name.text = _abilities[1].ToString();
        _selectPopup.SetActive(true);
        _selectButton.Select();
    }

    public void Ability1Selected()
    {
        var ability = new Ability(string.Empty, _abilities[0]);
        DataManager.Instance.SetAbilities(_newAbility, _abilities[1]);
        DataManager.Instance.Rewards[0, _rewardNum] = ability;
        _selectPopup.SetActive(false);
        Finished.Invoke();
    }

    public void Ability2Selected()
    {
        var ability = new Ability(string.Empty, _abilities[1]);
        DataManager.Instance.SetAbilities(_abilities[0], _newAbility);
        DataManager.Instance.Rewards[0, _rewardNum] = ability;
        _selectPopup.SetActive(false);
        Finished.Invoke();
    }
}
