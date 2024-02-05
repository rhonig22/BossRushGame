using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndScreenUXManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossScore;
    [SerializeField] TextMeshProUGUI timeScore;
    [SerializeField] TextMeshProUGUI damageScore;
    [SerializeField] TextMeshProUGUI totalScore;
    [SerializeField] GameObject nameView;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject nameEdit;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject playButton;
    private readonly float _waitTime = .5f;
    private readonly int _bossMultiplier = 500;
    private readonly int _damageMultiplier = 1;
    private readonly float _timePerBoss = 60;
    private int _score = 0;
    private string _name;

    private void Start()
    {
        _name = DataManager.Instance.GetName();
        if (_name == string.Empty)
        {
            ShowEditName(true);
            nameInput.Select();
        }
        else
            nameText.text = _name;

        StartCoroutine(LoadScores());
    }
    void Update()
    {
        if (nameInput.isFocused && Input.GetKeyUp(KeyCode.Return))
        {
            SubmitName();
        }
    }

    private IEnumerator LoadScores()
    {
        _score = 0;
        yield return new WaitForSeconds(_waitTime);
        bossScore.text = " " + DataManager.Instance.GetBossesDefeated();
        _score += DataManager.Instance.GetBossesDefeated() * _bossMultiplier;
        yield return new WaitForSeconds(_waitTime);
        timeScore.text = " " + DataManager.Instance.GetTime();
        _score += Mathf.FloorToInt((DataManager.Instance.GetBossesDefeated() + 1)* _timePerBoss - DataManager.Instance.GetTimeValue());
        yield return new WaitForSeconds(_waitTime);
        damageScore.text = " " + DataManager.Instance.GetDamageDealt();
        _score += DataManager.Instance.GetDamageDealt() * _damageMultiplier;
        yield return new WaitForSeconds(_waitTime);
        totalScore.text = " " + _score;
        if (_name != string.Empty)
            SubmitScore();
    }

    private void ShowEditName(bool show)
    {
        nameView.SetActive(!show);
        nameEdit.SetActive(show);
    }

    private void SubmitScore()
    {
        LeaderboardManager.Instance.SubmitLootLockerScore(_score);
    }

    public void StartGame()
    {
        GameManager.Instance.StartRun();
    }

    public void ViewLeaderboard()
    {
        GameManager.Instance.LoadLeaderboard();
    }

    public void EditName()
    {
        ShowEditName(true);
    }

    public void SubmitName()
    {
        _name = nameInput.text;
        LeaderboardManager.Instance.SetUserName(nameInput.text, (string name) => { 
            DataManager.Instance.SetName(_name);
            nameText.text = _name;
            ShowEditName(false);
            SubmitScore();
        });

        EventSystem.current.SetSelectedGameObject(playButton);
    }
}
