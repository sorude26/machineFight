using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NavigationMover : MonoBehaviour
{
    // [SerializeField]
    public Transform _target = default;
    [SerializeField]
    private float _naviInterval = 1f;
    [SerializeField]
    private float _moveSpeed = 1f;
    private Vector3 _currentDir = Vector3.zero;
    private Rigidbody _rb = default;
    private MyGame.MoveController _moveController = null;
    private float _timer = 0f;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _moveController = new MyGame.MoveController(_rb);
        _timer = _naviInterval;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _naviInterval)
        {
            _timer = 0;
            _currentDir = NavigationManager.Instance.GetMoveDir(transform);
        }
    }
    private void FixedUpdate()
    {
        if (_currentDir != Vector3.zero)
        {
            transform.forward = _currentDir;
        }
        _moveController.VelocityMove(_currentDir * _moveSpeed);
    }
}
