using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchAbility : BaseAbility
{
    private readonly float _scratchTime = .75f;
    private readonly int _scratchDamage = 20;
    private bool _enemyDamaged = false;
    [SerializeField] private GameObject _scratchArea;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        SetScratchPosition();
        _scratchArea.SetActive(true);
        DetectEnemyCollision();
        StartCoroutine(EndScratch());
    }
    private IEnumerator EndScratch()
    {
        yield return new WaitForSeconds(_scratchTime);
        if (!_enemyDamaged)
            DetectEnemyCollision();

        _scratchArea.SetActive(false);
        _enemyDamaged = false;
    }

    private void SetScratchPosition()
    {
        var direction = _playerController.CurrentDirection;
        if (Mathf.Abs(direction.x) > 0)
        {
            _scratchArea.transform.localScale = new Vector3(.5f, 1.5f, 1f);
            if (direction.x < 0)
                _scratchArea.transform.localPosition = new Vector3(-.75f, 0, 0);
            else
                _scratchArea.transform.localPosition = new Vector3(.75f, 0, 0);
        }
        else
        {
            _scratchArea.transform.localScale = new Vector3(1.5f, .5f, 1f);
            if (direction.y < 0)
                _scratchArea.transform.localPosition = new Vector3(0, -.75f, 0);
            else
                _scratchArea.transform.localPosition = new Vector3(0, .75f, 0);
        }

    }

    private void DetectEnemyCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_scratchArea.transform.position, _scratchArea.transform.localScale, 0, LayerMask.NameToLayer("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<BossHealth>().TakeDamage(_scratchDamage);
                _enemyDamaged = true;
            }
        }
    }
}
