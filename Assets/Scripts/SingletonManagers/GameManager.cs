using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private readonly string _roomName = "Boss_{0}";
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
        SceneManager.LoadScene(_roomName.Replace("{0}", _currentRoomId + ""));
    }
}
