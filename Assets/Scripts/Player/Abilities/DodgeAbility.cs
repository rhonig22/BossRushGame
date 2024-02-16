using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DodgeAbility : BaseAbility
{
    public static readonly float DodgeTime = .25f;
    private SpriteRenderer[] _spriteRenderers;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private TrailRenderer _trailRenderer;

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

        SoundManager.Instance.PlaySound(_dashSound, transform.position);
        _trailRenderer.emitting = true;
        StartCoroutine(EndDodge());
    }

    private IEnumerator EndDodge()
    {
        yield return new WaitForSeconds(DodgeTime);
        _playerController.EndDodge();
        _trailRenderer.emitting = false;
        foreach (SpriteRenderer renderer in _spriteRenderers)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
        }
    }
}
