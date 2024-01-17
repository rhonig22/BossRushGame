using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput, _verticalInput;
    private readonly int _speed = 15;
    private readonly float _movementSmoothing = .1f;
    private Vector2 _currentVelocity = Vector2.zero;
    [SerializeField] private Rigidbody2D _playerRB;

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Move(_horizontalInput, _verticalInput);
    }

    private void Move(float xSpeed, float ySpeed)
    {
        Vector3 targetDirection = new Vector3(xSpeed, ySpeed, 0).normalized;
        Vector3 targetVelocity = targetDirection * _speed;
        if (targetDirection.magnitude > 0)
            SetRotation(targetDirection);

        _playerRB.velocity = Vector2.SmoothDamp(_playerRB.velocity, targetVelocity, ref _currentVelocity, _movementSmoothing);
    }

    private void SetRotation(Vector3 direction)
    {
        _playerRB.transform.rotation = Quaternion.LookRotation(transform.forward, direction);
    }
}
