using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beeIdleBehavior : StateMachineBehaviour
{
    private BeeController _controller;
    private GameObject _player;
    private Vector2 _playerLocation = Vector2.zero;
    private Vector3 _startPosition, _endPosition;
    private float _elapsedTime, _percentageComplete;
    private float _desiredDuration = 1.5f;
    private float _angerDistance = 3f;
    [SerializeField] private AnimationCurve _curve;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = animator.GetComponent<BeeController>();
        _startPosition = _controller.transform.position;
        _endPosition = _controller.transform.position;
        _player = GameObject.FindWithTag("Player");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        // Check if the bee should be angry
        _playerLocation = _player.transform.position;
        var playerDistance = (Vector3)_playerLocation - _controller.transform.position;
        
        if(playerDistance.magnitude < _angerDistance)
        {
            animator.SetTrigger("alarmed");
        }

        // Check if destination was reached, then select new destination
        var destinationDistance = (Vector3)_controller.transform.position - _endPosition;
        if(Mathf.Abs(destinationDistance.magnitude) < 1)
        {
            _setNewTargetPosition();
            _elapsedTime = 0;
        }
        _elapsedTime += Time.deltaTime;
        _percentageComplete = _elapsedTime / _desiredDuration;
        _controller.transform.position = Vector3.Lerp(_startPosition, _endPosition, _curve.Evaluate(_percentageComplete));
    }
    private void _setNewTargetPosition()
    {
        _startPosition = _controller.transform.position;
        float targetX = Mathf.Clamp(_startPosition.x + (Random.Range(-100, 100)/50), -8.5f, 8.5f);
        float targetY = Mathf.Clamp(_startPosition.y + (Random.Range(-100, 100)/50), -5.5f, 5.5f);
        float targetZ = _startPosition.z;
        _endPosition = new Vector3(targetX, targetY, targetZ);
    }
}
