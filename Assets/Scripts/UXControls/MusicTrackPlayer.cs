using PlusMusicTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrackPlayer : MonoBehaviour
{
    public PMTransitionInfo arrangementTransition;
    public bool playOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
            PlayTrack();
    }

    public void PlayTrack()
    {
        MusicManager.Instance.AddTransition(arrangementTransition);
    }
}
