using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchAbility : BaseAbility
{
    private readonly int _scratchDamageMultiplier = 1;
    private bool _isScratching = false;
    [SerializeField] private GameObject _scratchArea;
    [SerializeField] private CapsuleCollider2D _scratchCollider;
    [SerializeField] private Animator _spriteAnimator;
    [SerializeField] private AudioClip _scratchMiss;
    [SerializeField] private AudioClip _scratchHit;
    public override void ActivateAbility()
    {
        if (_isScratching)
            return;

        base.ActivateAbility();
        _isScratching = true;
        _playerController.HaltMovement();
        SetScratchPosition();
        _scratchArea.SetActive(true);
        _spriteAnimator.SetTrigger("attack_scratch");
    }
    public void EndScratch()
    {
        _scratchArea.SetActive(false);
        _isScratching = false;
        _playerController.RestoreMovement();
    }

    private void SetScratchPosition()
    {
        var direction = _playerController.CurrentDirection;
        if (Mathf.Abs(direction.x) > 0)
        {
            _scratchCollider.size = new Vector2(1, 1);
            if (direction.x < 0)
            {
                _scratchArea.transform.localPosition = new Vector3(.1f, 0, 0);
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                _scratchArea.transform.localPosition = new Vector3(-.1f, 0, 0);
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
        else
        {
            _scratchArea.transform.localPosition = new Vector3(0, 0, 0);
            _scratchCollider.size = new Vector2(1, 1.4f);
            if (direction.y < 0)
            {
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }

    public void DetectEnemyCollision()
    {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        Physics2D.OverlapCollider(_scratchCollider, contactFilter, results);
        bool hit = false;
        foreach (Collider2D collider in results)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<BossHealth>().TakeDamage(DataManager.Instance.GetDamage(_scratchDamageMultiplier));
                hit = true;
            }
        }

        SoundManager.Instance.PlaySound(hit ? _scratchHit : _scratchMiss, transform.position);
    }
}
