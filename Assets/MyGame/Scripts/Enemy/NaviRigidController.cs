using MyGame.MachineFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviRigidController : MonoBehaviour
{
    [SerializeField]
    private Transform _body = default;
    [SerializeField]
    private Rigidbody _rb = default;
    [SerializeField]
    private int _naviPower = 0;
    [SerializeField]
    private float _diffusivity = 0.5f;
    [SerializeField]
    private float _moveDiffusivity = 0.98f;
    [SerializeField]
    private float _transSpeed = 5f;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _naviInterval = 1f;
    private float _timer = 0f;
    private Vector3 _currentDir = Vector3.zero;
    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer > _naviInterval)
        {
            _timer = 0;
            _currentDir = NavigationManager.Instance.GetMoveDir(_body,_naviPower);
            _currentDir.x += Random.Range(-_diffusivity, _diffusivity);
            _currentDir.z += Random.Range(-_diffusivity, _diffusivity);
        }
        if (_currentDir != Vector3.zero)
        {
            _body.forward = Vector3.Lerp(_body.forward, _currentDir, _transSpeed * Time.fixedDeltaTime);
            _rb.AddForce(_body.forward * _moveSpeed);
        }
        var velo = _rb.velocity;
        var g = velo.y;
        velo *= _moveDiffusivity;
        velo.y = g;
        _rb.velocity = velo;
    }
}
