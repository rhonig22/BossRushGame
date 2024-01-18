using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAbility : BaseAbility
{
    public static readonly float DodgeTime = .25f;
    private SpriteRenderer[] _spriteRenderers;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        _playerController.PerformDodge();
        foreach (SpriteRenderer renderer in _spriteRenderers)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, .5f);
        }

        StartCoroutine(EndDodge());
    }

    private IEnumerator EndDodge()
    {
        yield return new WaitForSeconds(DodgeTime);
        _playerController.EndDodge();
        foreach (SpriteRenderer renderer in _spriteRenderers)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
        }
    }
}
