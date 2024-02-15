using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleAbility : BaseAbility
{
    [SerializeField] private GameObject _bubble;
    [SerializeField] private AudioClip _bubbleSpitSound;
    private readonly float _bubbleDelay = 0.25f, _minMagnitude = 1f, _maxMagnitude = 7f, _minSize = .5f, _maxSize = 1.5f, _maxCharge = 1f;
    private float _magnitude = 1f, _timeCharged = 0;
    private bool _isBubbleCharging = false;
    private GameObject _tempBubble;
    private BubbleController _tempBubbleController;

    private void Update()
    {
        if (_isBubbleCharging)
        {
            _timeCharged += Time.deltaTime;
            _magnitude = Mathf.Lerp(_minMagnitude, _maxMagnitude, _timeCharged / _maxCharge);
            _tempBubble.transform.localScale = Vector3.Lerp(Vector3.one * _minSize, Vector3.one * _maxSize, _timeCharged / _maxCharge);
        }
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        _isBubbleCharging = true;
        _magnitude = _minMagnitude;
        _timeCharged = 0;
        CreateBubble();
        _playerController.HaltMovement();
    }

    public override void EndAbility()
    {
        base.EndAbility();
        SendBubble();
        _isBubbleCharging = false;
    }

    private void CreateBubble()
    {
        var bubble = Instantiate(_bubble);
        var bubbleController = bubble.GetComponent<BubbleController>();
        bubbleController.BubbleSent.AddListener(() => { EndBubble(); });
        SetBubblePosition(bubble);
        _tempBubble = bubble;
        _tempBubbleController = bubbleController;
    }

    private void SendBubble()
    {
        var endPosition = _tempBubble.transform.position + _playerController.CurrentDirection * _magnitude;
        SoundManager.Instance.PlaySound(_bubbleSpitSound, transform.position);
        _tempBubbleController.SendBubbleWithDelay(_bubbleDelay, endPosition);

        _tempBubble = null;
        _tempBubbleController = null;
    }

    private void EndBubble()
    {
        _playerController.RestoreMovement();
    }

    private void SetBubblePosition(GameObject bubble)
    {
        var direction = _playerController.CurrentDirection;
        bubble.transform.position = transform.position;
        bubble.transform.localScale = new Vector3(_minSize, _minSize, 1f);
        if (Mathf.Abs(direction.x) > 0)
        {
            if (direction.x < 0)
            {
                bubble.transform.localPosition += new Vector3(-.75f, 0, 0);
            }
            else
            {
                bubble.transform.localPosition += new Vector3(.75f, 0, 0);
            }
        }
        else
        {
            if (direction.y < 0)
            {
                bubble.transform.localPosition += new Vector3(0, -.75f, 0);
            }
            else
            {
                bubble.transform.localPosition += new Vector3(0, .75f, 0);
            }
        }

    }
}
