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
    /// <summary> 作戦範囲 </summary>
    public float OperationalRange { get => _operationalRange; }
    /// <summary> 離脱範囲 </summary>
    public float DetachmentRange { get => _detachmentRange + _operationalRange; }
    /// <summary> 最高高度 </summary>
    public float MaxHigh { get => _detachmentLevel + _operationalLevelHigh; }
    /// <summary> 最低高度 </summary>
    public float MaxLow { get =>  _operationalLevelLow - _detachmentLevel; }
    public bool IsOutOperatioanl;
    public bool IsInDetachment;
    public bool IsOutOperatioanlLevel;
    private bool _inStage = false;
    private float _inTime = 1f;
    private float _inTimer = 0f;
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
        if (IsInDetachment == true) { return; }
        CheckRange();
        CheckLevel();
    }
    private void CheckRange()
    {
        Vector3 playerPos = _player.position;
        playerPos.y = 0;
        if (Vector3.Distance(playerPos, _centerPos.position) < _operationalRange)
        {
            if (_inStage == false)
            {
                _inTimer += Time.fixedDeltaTime;
                if (_inTimer < _inTime)
                {
                    return;
                }
                _inStage = true;
                PopUpLog.CreatePopUp("作戦範囲内に到達しました");
            }
            IsOutOperatioanl = false;
            return;
        }
        if (_inStage == false)
        {
            return;
        }
        if (IsOutOperatioanl == false)
        {
            PopUpLog.CreatePopUp("作戦範囲外です");
        }
        IsOutOperatioanl = true;
        if (Vector3.Distance(playerPos, _centerPos.position) < _operationalRange + _detachmentRange)
        {
            return;
        }
        IsInDetachment = true;
        StageManager.Instance.ViewOutStage();
    }
    private void CheckLevel()
    {
        if (_player.position.y < _operationalLevelHigh && _player.position.y > _operationalLevelLow)
        {
            IsOutOperatioanlLevel = false;
            return;
        }
        if (_inStage == false)
        {
            return;
        }
        if (IsOutOperatioanlLevel == false)
        {
            PopUpLog.CreatePopUp("作戦範囲外高度です");
        }
        IsOutOperatioanlLevel = true;
        if (_player.position.y < _operationalLevelHigh + _detachmentLevel && _player.position.y > _operationalLevelLow - _detachmentLevel)
        {
            return;
        }
        IsInDetachment = true;
        StageManager.Instance.ViewOutStage();
    }
}
