using MyGame.MachineFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviMoveController : MonoBehaviour
{
    [SerializeField]
    private Transform _body = default;
    [SerializeField]
    private int _naviPower = 0;
    [SerializeField]
    private LayerMask _wallLayer = default;
    [SerializeField]
    private float _wallRayRange = 5f;
    [SerializeField]
    private float _diffusivity = 0.5f;
    [SerializeField]
    private float _transSpeed = 5f;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _naviInterval = 1f;
    [SerializeField]
    private float _minYLevel = 1f;
    [SerializeField]
    private float _maxYLevel = 500f;
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
            _currentDir.y += Random.Range(-_diffusivity, _diffusivity);
            _currentDir.z += Random.Range(-_diffusivity, _diffusivity);
        }        
        if (_currentDir != Vector3.zero)
        {
            if (Physics.Raycast(_body.position,_currentDir,_wallRayRange,_wallLayer))
            {
                _currentDir = -_currentDir;
            }
            if (_body.position.y <= _minYLevel)
            {
                _currentDir = Vector3.up;
            }
            if (_body.position.y >= _maxYLevel)
            {
                _currentDir = Vector3.down;
            }
            if (OperationalRangeManager.Instance != null)
            {
                var ope = OperationalRangeManager.Instance.transform.position;
                if (Vector3.Distance(ope, transform.position) > OperationalRangeManager.Instance.DetachmentRange)
                {
                    _currentDir = ope - transform.position;
                    _currentDir.y = 0;
                }
            }
            _body.forward = Vector3.Lerp(_body.forward, _currentDir, _transSpeed * Time.fixedDeltaTime);
            _body.position = Vector3.Lerp(_body.position, _body.position + _body.forward * _moveSpeed, _transSpeed * Time.fixedDeltaTime);
        }
    }
}
