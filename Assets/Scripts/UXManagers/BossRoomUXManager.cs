using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomUXManager : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;

    private void Start()
    {
        DataManager.Instance.StartTimer();
    }
}
