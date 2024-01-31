using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectPopupUXManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectPopup;
    [SerializeField] private TextMeshProUGUI _ability1Name;
    [SerializeField] private TextMeshProUGUI _ability2Name;
    [SerializeField] private Button _selectButton;
    private AbilityType _newAbility;
    private AbilityType[] _abilities;
    public UnityEvent Finished {  get; private set; } = new UnityEvent();

    public void Open(AbilityType newAbility)
    {
        _newAbility = newAbility;
        _abilities = DataManager.Instance.GetAbilities();
        _ability1Name.text = _abilities[0].ToString();
        _ability2Name.text = _abilities[1].ToString();
        _selectPopup.SetActive(true);
        _selectButton.Select();
    }

    public void Ability1Selected()
    {
        DataManager.Instance.SetAbilities(_newAbility, _abilities[1]);
        _selectPopup.SetActive(false);
        Finished.Invoke();
    }

    public void Ability2Selected()
    {
        DataManager.Instance.SetAbilities(_abilities[0], _newAbility);
        _selectPopup.SetActive(false);
        Finished.Invoke();
    }
}
