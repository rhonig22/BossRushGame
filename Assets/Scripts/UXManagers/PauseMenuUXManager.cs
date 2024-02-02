using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenuUXManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private TextMeshProUGUI _ability1Name;
    [SerializeField] private TextMeshProUGUI _ability2Name;
    public bool IsPaused { get; private set; } = false;
    public bool DoNotStartTimeOnUnPause = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    public void Pause()
    {
        IsPaused = !IsPaused;
        if (!DoNotStartTimeOnUnPause)
        {
            TimeManager.Instance.Pause(IsPaused);
        }
        else
        {
            DoNotStartTimeOnUnPause = false;
        }

        var abilities = DataManager.Instance.GetAbilities();
        _ability1Name.text = abilities[0].ToString();
        _ability2Name.text = abilities[1].ToString();
        _pauseMenu.SetActive(IsPaused);
    }
}
