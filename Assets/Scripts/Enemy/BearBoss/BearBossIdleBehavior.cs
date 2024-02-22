using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBossIdleBehavior : StateMachineBehaviour
{
    private float _attackTimer = 0f;
    private BearBossController _controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackTimer = 0;
        _controller = animator.GetComponent<BearBossController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller.Anger >= _controller.AngerThreshold)
        {
            _controller.AngerAttack();
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer > _controller.AttackWaitPeriod)
        {
            if (Random.Range(0f, 1f) >= _controller.AttackTriggerChance)
            {
                DetermineAttack();
            }
        }
    }

    private void DetermineAttack()
    {
        _controller.DetermineNextAttack();
    }
}
