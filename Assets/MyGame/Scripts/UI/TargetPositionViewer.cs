using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPositionViewer : MonoBehaviour
{
    private static readonly float ARROW_RANGE = 0.8f;
    [SerializeField]
    private GameObject _frontArrow = default;
    [SerializeField]
    private GameObject _leftArrow = default;
    [SerializeField]
    private GameObject _rightArrow = default;
    [SerializeField]
    private GameObject _backArrow = default;
    [SerializeField]
    private float _updateInterval = 0.5f;
    private float _timer = 0;
    private Transform _player = default;
    private void Start()
    {
        _player = NavigationManager.Instance.Target;
    }
    private void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer < 0)
        {
            _timer = _updateInterval;
            ViewArrow();
        }
    }
    private void ViewArrow()
    {
        _frontArrow.SetActive(false);
        _rightArrow.SetActive(false);
        _leftArrow.SetActive(false);
        _backArrow.SetActive(false);
        CheckForward();
        CheckLeft();
        CheckRight();
        CheckBack();
    }
    private void CheckForward()
    {
        foreach (var target in LockOnController.Instance.LockOnTargets)
        {
            if (target.DamageChecker.AddTarget == false || target.DamageChecker.gameObject.activeInHierarchy == false)
            {
                continue;
            }
            Vector3 dir = target.DamageChecker.transform.position - _player.position;
            dir = dir.normalized;
            dir.y = _player.position.y;
            if (Vector3.Dot(dir,_player.forward) > ARROW_RANGE)
            {
                _frontArrow.SetActive(true);
                return;
            }
        }
    }
    private void CheckBack()
    {
        foreach (var target in LockOnController.Instance.LockOnTargets)
        {
            if (target.DamageChecker.AddTarget == false || target.DamageChecker.gameObject.activeInHierarchy == false)
            {
                continue;
            }
            Vector3 dir = target.DamageChecker.transform.position - _player.position;
            dir = dir.normalized;
            dir.y = _player.position.y;
            if (Vector3.Dot(dir,-_player.forward) > ARROW_RANGE)
            {
                _backArrow.SetActive(true);
                return;
            }
        }
    }
    private void CheckRight()
    {
        foreach (var target in LockOnController.Instance.LockOnTargets)
        {
            if (target.DamageChecker.AddTarget == false || target.DamageChecker.gameObject.activeInHierarchy == false)
            {
                continue;
            }
            Vector3 dir = target.DamageChecker.transform.position - _player.position;
            dir = dir.normalized;
            dir.y = _player.position.y;
            if (Vector3.Dot(dir, _player.right) > ARROW_RANGE)
            {
                _rightArrow.SetActive(true);
                return;
            }
        }
    }
    private void CheckLeft()
    {
        foreach (var target in LockOnController.Instance.LockOnTargets)
        {
            if (target.DamageChecker.AddTarget == false || target.DamageChecker.gameObject.activeInHierarchy == false)
            {
                continue;
            }
            Vector3 dir = target.DamageChecker.transform.position - _player.position;
            dir = dir.normalized;
            dir.y = _player.position.y;
            if (Vector3.Dot(dir, -_player.right) > ARROW_RANGE)
            {
                _leftArrow.SetActive(true);
                return;
            }
        }
    }
}
