using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private readonly string _roomName = "Boss_{0}";
    private readonly string _endSceneName = "EndScreen";
    private readonly string _transitionSceneName = "TransitionScreen";
    private readonly int _maxBosses = 1;
    private int _currentRoomId = 1;
    public int Difficulty { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartRun()
    {
        DataManager.Instance.ResetData();
        _currentRoomId = 1;
        Difficulty = 1;
        LoadNextBoss();
    }

    public void DefeatedBoss()
    {
        _currentRoomId++;
        DataManager.Instance.AddBossDefeated();
        if (_currentRoomId > _maxBosses)
        {
            _currentRoomId -= _maxBosses;
            Difficulty += 1;
        }

        LoadRewards();
    }

    public void EndRun()
    {
        DataManager.Instance.PauseTimer();
        SceneManager.LoadScene(_endSceneName);
    }

    public void LoadRewards()
    {
        DataManager.Instance.PauseTimer();
        SceneManager.LoadScene(_transitionSceneName);
    }

    public void LoadNextBoss()
    {
        SceneManager.LoadScene(_roomName.Replace("{0}", _currentRoomId + ""));
    }
}
