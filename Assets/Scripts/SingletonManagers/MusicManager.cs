using PlusMusic;
using PlusMusicTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private float _volume = 1f;
    private bool _hasProjectLoaded = false;
    private bool _isTrackLoaded = false;
    private List<PMTransitionInfo> _nextArrangements = new List<PMTransitionInfo>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        TryLoadTracks();
    }

    private void Update()
    {
        if (!_hasProjectLoaded)
        {
            TryLoadTracks();
        }

        if (_isTrackLoaded && _nextArrangements.Count > 0)
        {
            var arrangement = _nextArrangements[0];
            _nextArrangements.Remove(arrangement);
            LoadArrangement(arrangement);
        }
    }

    public void ChangeMasterVolume(float volume)
    {
        _volume = volume;
    }

    private void TryLoadTracks()
    {
        if (PlusMusicCore.Instance.GetIsProjectLoaded)
        {
            _hasProjectLoaded = true;
            LoadTrack();
        }
    }

    private void LoadArrangement(PMTransitionInfo arrangementInfo)
    {
        PlusMusicCore.Instance.PlayArrangement(arrangementInfo);
    }

    public void LoadTrack()
    {
        PlusMusicCore.Instance.OnTrackLoadingProgress += TrackLoadingProgress;
        PlusMusicCore.Instance.LoadTrack(new PMTrackProgress
        {
            id = 0,
            index = 0,
            autoPlay = false
        });
    }

    private void TrackLoadingProgress(PMTrackProgress progress)
    {
        if (progress.progress < 1.0f)
            _isTrackLoaded = false;
        else
            _isTrackLoaded = true;
    }

    public void AddTransition(PMTransitionInfo transitionInfo)
    {
        _nextArrangements.Add(transitionInfo);
    }
}
