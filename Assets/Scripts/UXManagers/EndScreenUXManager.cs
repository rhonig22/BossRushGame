using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenUXManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossScore;
    [SerializeField] TextMeshProUGUI timeScore;
    [SerializeField] TextMeshProUGUI damageScore;
    [SerializeField] TextMeshProUGUI healthScore;
    [SerializeField] TextMeshProUGUI totalScore;
    private float _waitTime = .5f;

    private void Start()
    {
        StartCoroutine(LoadScores());
    }

    private IEnumerator LoadScores()
    {
        yield return new WaitForSeconds(_waitTime);
        bossScore.text = " " + DataManager.Instance.GetBossesDefeated();
        yield return new WaitForSeconds(_waitTime);
        timeScore.text = " " + DataManager.Instance.GetTime();
        yield return new WaitForSeconds(_waitTime);
        damageScore.text = " " + DataManager.Instance.GetDamageDealt();
        yield return new WaitForSeconds(_waitTime);
        healthScore.text = " " + DataManager.Instance.GetHealth();
        yield return new WaitForSeconds(_waitTime);
        totalScore.text = " TBD";
    }

    public void StartGame()
    {
        GameManager.Instance.StartRun();
    }
}
