using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenuUXManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private TextMeshProUGUI _ability1Name;
    [SerializeField] private TextMeshProUGUI _ability2Name;
    public bool IsPaused { get; private set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    private void Pause()
    {
        IsPaused = !IsPaused;
        TimeManager.Instance.Pause(IsPaused);
        var abilities = DataManager.Instance.GetAbilities();
        _ability1Name.text = abilities[0].ToString();
        _ability2Name.text = abilities[1].ToString();
        _pauseMenu.SetActive(IsPaused);
    }
}
