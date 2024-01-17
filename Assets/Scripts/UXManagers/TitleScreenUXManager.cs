using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUXManager : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartRun();
    }
}
