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
    private GameObject _frontBossArrow = default;
    [SerializeField]
    private GameObject _leftBossArrow = default;
    [SerializeField]
    private GameObject _rightBossArrow = default;
    [SerializeField]
    private GameObject _backBossArrow = default;
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
        _frontBossArrow.SetActive(false);
        _rightBossArrow.SetActive(false);
        _leftBossArrow.SetActive(false);
        _backBossArrow.SetActive(false);
        foreach (var target in LockOnController.Instance.LockOnTargets)
        {
            if (target.DamageChecker.AddTarget == false || target.DamageChecker.gameObject.activeInHierarchy == false)
            {
                continue;
            }
            CheckDir(target.DamageChecker.transform.position, _player.forward, _frontArrow);
            CheckDir(target.DamageChecker.transform.position, -_player.forward, _backArrow);
            CheckDir(target.DamageChecker.transform.position, _player.right, _rightArrow);
            CheckDir(target.DamageChecker.transform.position, -_player.right, _leftArrow);
        }
        foreach (var target in LockOnController.Instance.LockOnTargets)
        {
            if (target.DamageChecker.BossTarget == false || target.DamageChecker.gameObject.activeInHierarchy == false)
            {
                continue;
            }
            CheckDir(target.DamageChecker.transform.position, _player.forward,_frontBossArrow);
            CheckDir(target.DamageChecker.transform.position, -_player.forward, _backBossArrow);
            CheckDir(target.DamageChecker.transform.position, _player.right, _rightBossArrow);
            CheckDir(target.DamageChecker.transform.position, -_player.right, _leftBossArrow);
        }
    }
    private bool CheckDir(Vector3 target,Vector3 checkDir,in GameObject obj)
    {
        Vector3 dir = target - _player.position;
        dir = dir.normalized;
        dir.y = _player.position.y;
        if (Vector3.Dot(dir, checkDir) > ARROW_RANGE)
        {
            obj.SetActive(true);
            return true;
        }
        return false;
    }
}
