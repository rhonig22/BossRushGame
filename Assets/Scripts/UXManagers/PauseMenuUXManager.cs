using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUXManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
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
        _pauseMenu.SetActive(IsPaused);
    }
}
