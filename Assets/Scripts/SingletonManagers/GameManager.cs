using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private readonly string _roomName = "Boss_{0}";
    private readonly string _endSceneName = "EndScreen";
    private readonly int _maxBosses = 1;
    private int _currentRoomId = 1;

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
        LoadBoss(_currentRoomId);
    }

    public void DefeatedBoss()
    {
        _currentRoomId++;
        DataManager.Instance.AddBossDefeated();
        if (_currentRoomId <= _maxBosses)
            LoadBoss(_currentRoomId);
        else
            EndRun();
    }

    public void EndRun()
    {
        DataManager.Instance.PauseTimer();
        SceneManager.LoadScene(_endSceneName);
    }

    private void LoadBoss(int roomId)
    {
        SceneManager.LoadScene(_roomName.Replace("{0}", roomId + ""));
    }
}
