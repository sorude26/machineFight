using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spwanPoints = default;
    [SerializeField]
    private GeneratorEnemyContoroller[] _enemys = default;
    [SerializeField]
    private int _defaultCount = 1;
    [SerializeField]
    private int _startSpwanCount = 2;
    [SerializeField]
    private float _minSpwanRange = 800f;
    [SerializeField]
    private int _enemySpwanCount = 5;
    [SerializeField]
    private int _enemySpwanTargetCount = 5;
    [SerializeField]
    private bool _isAutoSpwan = true;
    private int _enemyIndex = 0;
    private int _pointIndex = 0;
    private int _count = 0;
    private int _totalCount = 0;
    private bool _isStart = false;
    private GeneratorEnemyContoroller[] _allEnemys = default;
    private IEnumerator Start()
    {
        _allEnemys = new GeneratorEnemyContoroller[_enemys.Length * _defaultCount];
        for (int i = 0; i < _enemys.Length; i++)
        {
            for (int k = 0; k < _defaultCount; k++)
            {
                _allEnemys[i * _defaultCount + k] = Instantiate(_enemys[i],transform);
                if (_isAutoSpwan == true)
                {
                    _allEnemys[i * _defaultCount + k].OnDeadEvent += AddCount;
                }
                _allEnemys[i * _defaultCount + k].gameObject.SetActive(false);
            }
        }
        yield return null;
        Spwan();
        for (int i = 0; i < _startSpwanCount; i++)
        {
            SpwanEnemys();
        }
        _isStart = true;
    }
    public void Spwan()
    {
        ShufflePoints();
        SpwanEnemys();
    }
    private void AddCount()
    {
        if (_isStart == false)
        {
            return;
        }
        _count++;
        if (_count >= _enemySpwanTargetCount)
        {
            SpwanEnemys();
            _count = 0;
        }
        _totalCount++;
        if (_totalCount >= _allEnemys.Length)
        {
            _totalCount = 0;
            ShuffleEnemys();
            ShufflePoints();
        }
    }
    private void SpwanEnemys()
    {
        for (int i = 0; i < _enemySpwanCount; i++)
        {
            NextPoint();
            if (NavigationManager.Instance.Target == null)
            {
                return;
            }
            if (Vector3.Distance(_spwanPoints[_pointIndex].position, NavigationManager.Instance.Target.position) > _minSpwanRange)
            {
                SpwanEnemy(_spwanPoints[_pointIndex]);
            }
        }
    }
    private void ShufflePoints()
    {
        for (int i = 0; i < _spwanPoints.Length; i++)
        {
            int r = Random.Range(0,_spwanPoints.Length);
            var pos = _spwanPoints[i];
            _spwanPoints[i] = _spwanPoints[r];
            _spwanPoints[r] = pos;
        }
    }
    private void ShuffleEnemys()
    {
        for (int i = 0; i < _allEnemys.Length; i++)
        {
            int r = Random.Range(0, _allEnemys.Length);
            var pos = _allEnemys[i];
            _allEnemys[i] = _allEnemys[r];
            _allEnemys[r] = pos;
        }
    }
    private void SpwanEnemy(Transform pos)
    {
        NextEnemy();
        if (_allEnemys[_enemyIndex].IsActive == false)
        {
            _allEnemys[_enemyIndex].InitializeEnemy(pos);
        }
    }
    private void NextEnemy()
    {
        for (int i = 0; i < _allEnemys.Length; i++)
        {
            if (_allEnemys[_enemyIndex].IsActive == false)
            {
                return;
            }
            _enemyIndex++;
            if (_enemyIndex >= _allEnemys.Length)
            {
                _enemyIndex = 0;
            }
        }
    }
    private void NextPoint()
    {
        for (int i = 0; i < _spwanPoints.Length; i++)
        {
            if (NavigationManager.Instance.Target == null)
            {
                return;
            }
            if (Vector3.Distance(_spwanPoints[i].position,NavigationManager.Instance.Target.position) > _minSpwanRange)
            {
                return;
            }
            _pointIndex++;
            if (_pointIndex >= _spwanPoints.Length)
            {
                _pointIndex = 0;
            }
        }
    }
}
