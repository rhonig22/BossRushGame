using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAngryBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private BeeController _controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = animator.GetComponent<BeeController>();
        _controller.isAngry = true; 
    }
}
