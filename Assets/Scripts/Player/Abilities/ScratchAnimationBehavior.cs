using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchAnimationBehavior : StateMachineBehaviour
{
    private ScratchAbility _scratchAbility;
    private float _timePassed = 0f;
    private bool _scratched = false;
    private readonly float _attackTime = .03f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _scratchAbility = GameObject.FindWithTag("Player").GetComponent<ScratchAbility>();
        _timePassed = 0;
        _scratched = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timePassed += Time.deltaTime;
        if (!_scratched && _timePassed > _attackTime)
        {
            _scratchAbility.DetectEnemyCollision();
            _scratched=true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _scratchAbility.EndScratch();
    }
}
