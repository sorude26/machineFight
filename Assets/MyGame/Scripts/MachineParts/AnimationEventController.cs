using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// アニメーションイベントで呼ばれる機能を持つ
/// </summary>
public class AnimationEventController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _playEvent = default;
    [SerializeField]
    private ShakeParam _shakeParam = default;
    [SerializeField]
    private ShakeParam _strongParam = default;
    private void PlayEvent()
    {
        _playEvent?.Invoke();
    }
    private void PlayShake()
    {
        StageShakeController.PlayShake(transform.position + _shakeParam.Pos, _shakeParam.Power, _shakeParam.Time);
    }
    private void PlayStrongShake()
    {
        StageShakeController.PlayShake(transform.position + _strongParam.Pos, _strongParam.Power, _strongParam.Time);
    }
}
