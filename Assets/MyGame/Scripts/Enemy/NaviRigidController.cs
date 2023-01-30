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
    private MyGame.MoveController _controller = null;
    private void Start()
    {
        _controller = new MyGame.MoveController(_rb);
    }
    private void FixedUpdate()
    {
        if (StageManager.InStage == false) { return; }
        _timer += Time.fixedDeltaTime;
        if (_timer > _naviInterval)
        {
            _timer = 0;
            _currentDir = NavigationManager.Instance.GetMoveDir(_body, _naviPower);
        }
        if (_currentDir != Vector3.zero)
        {
            if (OperationalRangeManager.Instance != null)
            {
                var ope = OperationalRangeManager.Instance.transform.position;
                if (Vector3.Distance(ope, transform.position) > OperationalRangeManager.Instance.DetachmentRange)
                {
                    _currentDir = ope - transform.position;
                    _currentDir.y = 0;
                }
            }
            _currentDir.x += Random.Range(-_diffusivity, _diffusivity);
            _currentDir.z += Random.Range(-_diffusivity, _diffusivity);
            _body.forward = Vector3.Lerp(_body.forward, _currentDir.normalized, _transSpeed * Time.fixedDeltaTime);
            _controller.GVelocityMove(_body.forward * _moveSpeed);
        }
        _controller.MoveDecelerate(_moveDiffusivity);
    }
    public void LockTarget()
    {
        var dir = NavigationManager.Instance.Target.position - _body.position;
        dir.y = 0;
        _currentDir = dir.normalized;
    }
}
