using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationalRangeManager : MonoBehaviour
{
    [SerializeField]
    private Transform _centerPos = default;
    [SerializeField]
    private float _operationalRange = 500f;
    [SerializeField]
    private float _detachmentRange = 10f;
    [SerializeField]
    private float _operationalLevelHigh = 500f;
    [SerializeField]
    private float _operationalLevelLow = -5f;
    [SerializeField]
    private float _detachmentLevel = 5f;
    private Transform _player = default;
    /// <summary> çÏêÌîÕàÕ </summary>
    public float OperationalRange { get => _operationalRange; }
    /// <summary> ó£íEîÕàÕ </summary>
    public float DetachmentRange { get => _detachmentRange + _operationalRange; }
    /// <summary> ç≈çÇçÇìx </summary>
    public float MaxHigh { get => _detachmentLevel + _operationalLevelHigh; }
    /// <summary> ç≈í·çÇìx </summary>
    public float MaxLow { get =>  _operationalLevelLow - _detachmentLevel; }
    public bool IsOutOperatioanl;
    public bool IsInDetachment;
    private void Start()
    {
        _player = NavigationManager.Instance.Target;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_centerPos.position, _operationalRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_centerPos.position, _operationalRange + _detachmentRange);
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = _player.transform.position;
        playerPos.y = 0;
        if (Vector3.Distance(playerPos, _centerPos.position) < _operationalRange)
        {
            IsOutOperatioanl = false;
            return;
        }
        IsOutOperatioanl = true;
        if (Vector3.Distance(playerPos, _centerPos.position) < _operationalRange + _detachmentRange)
        {
            return;
        }
        IsInDetachment = true;
    }
    private void CheckRange()
    {
        Vector3 playerPos = _player.position;
        playerPos.y = 0;
        if (Vector3.Distance(playerPos, _centerPos.position) < _operationalRange)
        {
            IsOutOperatioanl = false;
            return;
        }
        IsOutOperatioanl = true;
        if (Vector3.Distance(playerPos, _centerPos.position) < _operationalRange + _detachmentRange)
        {
            return;
        }
        IsInDetachment = true;
    }
    private void CheckLevel()
    {
        if (_player.position.y < _operationalLevelHigh && _player.position.y > _operationalLevelLow)
        {
            IsOutOperatioanl = false;
            return;
        }
        IsOutOperatioanl = true;
        if (_player.position.y < _operationalLevelHigh + _detachmentLevel && _player.position.y > _operationalLevelLow - _detachmentLevel)
        {
            return;
        }
        IsInDetachment = true;
    }
}
