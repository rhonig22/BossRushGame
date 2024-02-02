using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomUXManager : MonoBehaviour
{
    [SerializeField] private CutSceneManager _cutSceneManager;
    [SerializeField] private PauseMenuUXManager _pauseMenuManager;
    private bool _cutScenePlayed = false;

    private void Start()
    {
        if (DataManager.Instance.ShouldPauseAtStart)
        {
            DataManager.Instance.InitialPauseComplete();
            _pauseMenuManager.Pause();
            _pauseMenuManager.DoNotStartTimeOnUnPause = true;
        }
        else
            StartCutscene();
    }

    private void Update()
    {
        if (!_pauseMenuManager.IsPaused && !_cutScenePlayed)
            StartCutscene();
    }

    private void StartCutscene()
    {
        _cutScenePlayed = true;
        _cutSceneManager.Play();
    }
}
