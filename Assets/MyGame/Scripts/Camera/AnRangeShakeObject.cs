using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnRangeShakeObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 _shakeAxis = Vector3.one;
    [SerializeField]
    private float _shakeRange = 5f;
    [SerializeField]
    private float _maxRange = 5f;

    private Vector3 _shakePos = default;
    private float _startPower = default;
    private float _shakeTimer = 0;
    private bool _isPlaying = false;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private void OnEnable()
    {
        StageShakeController.OnPlayStageShake += PlayShake;
    }
    private void OnDisable()
    {
        StageShakeController.OnPlayStageShake -= PlayShake;
    }
    /// <summary>
    /// êkåπínÇ©ÇÁÇÃãóó£Ç…âûÇ∂ÇƒêUìÆÇ∑ÇÈ
    /// </summary>
    /// <param name="shakePos">êkåπç¿ïW</param>
    /// <param name="power">êUìÆÇÃã≠Ç≥</param>
    /// <param name="shakeTime">éùë±éûä‘</param>
    private void PlayShake(Vector3 shakePos, float power, float shakeTime)
    {
        if (_isPlaying)
        {
            if (_startPower > power)
            {
                _startPower = power;
                _shakeTimer = shakeTime;
            }
            return;
        }
        _isPlaying = true;
        StartCoroutine(ShakeImpl(power, shakeTime));
    }
    private IEnumerator ShakeImpl(float power, float shakeTime)
    {
        _startPower = power;
        _shakeTimer = shakeTime;
        _shakePos = Vector3.zero;
        while (_shakeTimer > 0)
        {
            float range = _startPower * _shakeRange;
            if (range > _maxRange)
            {
                range = _maxRange;
            }
            _shakePos.x += _shakeAxis.x * range * (1 - 2 * Random.Range(0, 2));
            _shakePos.y += _shakeAxis.y * range * (1 - 2 * Random.Range(0, 2));
            _shakePos.z += _shakeAxis.z * range * (1 - 2 * Random.Range(0, 2));
            transform.localPosition = _shakePos;
            _shakePos = Vector3.zero;
            _shakeTimer -= Time.fixedDeltaTime;
            yield return _waitForFixedUpdate;
        }
        transform.localPosition = Vector3.zero;
        _isPlaying = false;
    }
}
