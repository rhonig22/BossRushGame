using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchAbility : BaseAbility
{
    private readonly float _scratchTime = .75f;
    [SerializeField] private GameObject _scratchArea;

    private void Awake()
    {
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        _scratchArea.SetActive(true);
        StartCoroutine(EndScratch());
    }
    private IEnumerator EndScratch()
    {
        yield return new WaitForSeconds(_scratchTime);
        _scratchArea.SetActive(false);
    }
}
