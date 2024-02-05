using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private Animator _cutSceneAnimator;
    [SerializeField] private GameObject _cutSceneCanvas;
    [SerializeField] private AudioClip _emphasis1;
    [SerializeField] private AudioClip _emphasis2;
    [SerializeField] private AudioClip _emphasis3;
    [SerializeField] private AudioClip _bell;
    [SerializeField] private MusicTrackPlayer _musicPlayer;
    [SerializeField] private TextMeshProUGUI _bossNumber;
    [SerializeField] private TextMeshProUGUI _bossName;

    public void Play()
    {
        TimeManager.Instance.Pause(true);
        _bossNumber.text = "Boss #" + (DataManager.Instance.GetBossesDefeated() + 1);
        _bossName.text = DataManager.Instance.FrogBossNames[Mathf.Clamp(DataManager.Instance.GetBossesDefeated(), 0, DataManager.Instance.FrogBossNames.Length - 1)];
        MusicManager.Instance.StopMusicClip();
        _cutSceneAnimator.SetTrigger("StartRoom");
    }

    public void Finished()
    {
        _cutSceneCanvas.SetActive(false);
        DataManager.Instance.StartTimer();
        TimeManager.Instance.Pause(false);
        SoundManager.Instance.PlaySound(_bell, transform.position);
        _musicPlayer.PlayTrack();
    }

    public void PlayEmphasis1()
    {
        SoundManager.Instance.PlaySound(_emphasis1, transform.position);
    }

    public void PlayEmphasis2()
    {
        SoundManager.Instance.PlaySound(_emphasis2, transform.position);
    }

    public void PlayEmphasis3()
    {
        SoundManager.Instance.PlaySound(_emphasis3, transform.position);
    }
}
