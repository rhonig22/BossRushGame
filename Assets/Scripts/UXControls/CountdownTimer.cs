using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] Slider Timer;
    private readonly float _lerpSpeed = .5f;
    private float _maxTime, _timeVal = 0;
    private bool _isStarted = false;
    public UnityEvent TimerFinished = new UnityEvent();

    private void Update()
    {
        if (_isStarted)
        {
            _timeVal = Mathf.MoveTowards(_timeVal, _maxTime, _lerpSpeed * Time.deltaTime);
            Timer.value = (_maxTime - _timeVal) / _maxTime;
        }

        if (Timer.value <= 0)
        {
            TimerFinished.Invoke();
        }
    }
    public void StartTimer(float time)
    {
        _maxTime = time;
        _isStarted = true;
    }
    public void StopTimer()
    {
        _isStarted = false;
    }
}
