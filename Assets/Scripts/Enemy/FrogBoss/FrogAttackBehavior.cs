using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAttackBehavior : StateMachineBehaviour
{
    [SerializeField] private FrogAttackType _attackType;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = animator.GetComponent<FrogBossController>();
        controller.StartAttack(_attackType);
    }
}
