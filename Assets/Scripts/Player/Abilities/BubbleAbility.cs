using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAbility : BaseAbility
{
    [SerializeField] private GameObject _bubble;
    [SerializeField] private AudioClip _bubbleSpitSound;
    private float _bubbleDelay = 0.25f;
    private float _magnitude = 6f;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        CreateBubble();
        _playerController.HaltMovement();
    }

    private void CreateBubble()
    {
        var bubble = Instantiate(_bubble);
        var bubbleController = bubble.GetComponent<BubbleController>();
        bubbleController.BubbleSent.AddListener(() => { EndBubble(); });
        SetBubblePosition(bubble);
        var endPosition = bubble.transform.position + _playerController.CurrentDirection * _magnitude;
        SoundManager.Instance.PlaySound(_bubbleSpitSound, transform.position);
        StartCoroutine(bubbleController.SendBubble(_bubbleDelay, endPosition));
    }

    private void EndBubble()
    {
        _playerController.RestoreMovement();
    }

    private void SetBubblePosition(GameObject bubble)
    {
        var direction = _playerController.CurrentDirection;
        bubble.transform.position = transform.position;
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
