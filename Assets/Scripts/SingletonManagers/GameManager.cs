using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private readonly float _transitionTime = 1.5f;
    private readonly string _roomName = "Boss_{0}";
    private readonly string _endSceneName = "EndScreen";
    private readonly string _transitionSceneName = "TransitionScreen";
    private readonly string _leaderboardSceneName = "Leaderboard";
    private readonly int _maxBosses = 1;
    private int _currentRoomId = 1;
    private UnityEvent _sceneTransition = new UnityEvent();

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

    // called second
    private void OnLevelWasLoaded(int level)
    {
        TransitionManager.Instance.FadeIn(() => { });
    }

    public void StartRun()
    {
        DataManager.Instance.PauseTimer();
        DataManager.Instance.ResetData();
        _currentRoomId = 1;
        LoadNextBoss();
    }

    public void DefeatedBoss()
    {
        DataManager.Instance.PauseTimer();
        _currentRoomId++;
        DataManager.Instance.AddBossDefeated();
        if (_currentRoomId > _maxBosses)
        {
            _currentRoomId -= _maxBosses;
            DataManager.Instance.IncreaseDifficulty();
        }

        LoadRewards();
    }

    public void EndRun()
    {
        DataManager.Instance.PauseTimer();
        LoadEndScene();
    }

    private void LoadEndScene()
    {
        UnityAction loadEndScene = () => { SceneManager.LoadScene(_endSceneName); };
        StartCoroutine(WaitAndTransition(loadEndScene, _transitionTime));
    }

    public void LoadLeaderboard()
    {
        UnityAction loadEndScene = () => { SceneManager.LoadScene(_leaderboardSceneName); };
        StartCoroutine(WaitAndTransition(loadEndScene, 0f));
    }

    private void LoadRewards()
    {
        UnityAction loadRewards = () => { SceneManager.LoadScene(_transitionSceneName); };
        StartCoroutine(WaitAndTransition(loadRewards, _transitionTime));
    }

    public void LoadNextBoss()
    {
        UnityAction loadNextBoss = () => { SceneManager.LoadScene(_roomName.Replace("{0}", _currentRoomId + "")); };
        StartCoroutine(WaitAndTransition(loadNextBoss, 0f));
    }

    private IEnumerator WaitAndTransition(UnityAction action, float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);
        _sceneTransition.RemoveAllListeners();
        _sceneTransition.AddListener(action);
        TransitionManager.Instance.FadeOut(() => { _sceneTransition.Invoke(); });
    }
}
