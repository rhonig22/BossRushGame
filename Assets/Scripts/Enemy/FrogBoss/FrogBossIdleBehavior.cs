using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBossIdleBehavior : StateMachineBehaviour
{
    private readonly float _attackWaitPeriod = 1.5f;
    private readonly float _chanceOfAttack = .4f;
    private float _attackTimer = 0f;
    private FrogBossController _controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackTimer = 0;
        _controller = animator.GetComponent<FrogBossController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer > _attackWaitPeriod)
        {
            if (Random.Range(0f, 1f) >= _chanceOfAttack)
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
