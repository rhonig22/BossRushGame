using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchAbility : BaseAbility
{
    private readonly int _scratchDamageMultiplier = 1;
    private bool _isScratching = false;
    [SerializeField] private GameObject _scratchArea;
    [SerializeField] private Animator _spriteAnimator;
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
            if (direction.x < 0)
            {
                _scratchArea.transform.localPosition = new Vector3(-.75f, 0, 0);
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                _scratchArea.transform.localPosition = new Vector3(.75f, 0, 0);
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            if (direction.y < 0)
            {
                _scratchArea.transform.localPosition = new Vector3(0, -.75f, 0);
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                _scratchArea.transform.localPosition = new Vector3(0, .75f, 0);
                _scratchArea.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }

    public void DetectEnemyCollision()
    {
        var capsule = _scratchArea.GetComponent<CapsuleCollider2D>();
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll((Vector2)_scratchArea.transform.position + capsule.offset, capsule.size, CapsuleDirection2D.Vertical, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<BossHealth>().TakeDamage(DataManager.Instance.GetDamage(_scratchDamageMultiplier));
            }
        }
    }
}
