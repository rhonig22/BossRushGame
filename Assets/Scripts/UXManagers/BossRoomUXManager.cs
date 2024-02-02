using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomUXManager : MonoBehaviour
{
    [SerializeField] private CutSceneManager _cutSceneManager;

    private void Start()
    {
        _cutSceneManager.Play();
    }
}
