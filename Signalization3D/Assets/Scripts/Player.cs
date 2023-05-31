using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private const string AnimationBlendTreeParametr = "movementSpeed";
    private Animator _animator;
    private Rigidbody _rigidbody;
    private PlayerInput _input;
    private Vector2 _direction;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Enable();

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _direction = _input.Player.Move.ReadValue<Vector2>();

        Move(_direction);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.1)
        {
            _animator.SetFloat(AnimationBlendTreeParametr, 0);
            return; 
        }    

        Vector3 moveDirectionVector = new Vector3(direction.x, 0, direction.y);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirectionVector), Time.deltaTime * _rotateSpeed);
       
        transform.position += moveDirectionVector * _moveSpeed * Time.deltaTime;

        _animator.SetFloat(AnimationBlendTreeParametr, Vector3.ClampMagnitude(moveDirectionVector, 1).magnitude);
    }
}
