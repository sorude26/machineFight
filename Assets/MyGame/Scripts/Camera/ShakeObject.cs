using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ローカル座標で振動を行うスクリプト
/// </summary>
public class ShakeObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 _shakeAxis = Vector3.one;
    [SerializeField]
    private float _shakeRange = 5f;
    [SerializeField]
    private float _maxRange = 5f;

    private Vector3 _startPos = default;
    private Vector3 _shakePos = default;
    private float _startPower = default;
    private float _shakeTimer = 0;
    private float _shakeDistance = default;
    private bool _isPlaying = false;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private void OnEnable()
    {
        _startPos = transform.localPosition;
        StageShakeController.OnPlayStageShake += PlayShake;
    }
    private void OnDisable()
    {
        StageShakeController.OnPlayStageShake -= PlayShake;
    }
    /// <summary>
    /// 震源地からの距離に応じて振動する
    /// </summary>
    /// <param name="shakePos">震源座標</param>
    /// <param name="power">振動の強さ</param>
    /// <param name="shakeTime">持続時間</param>
    private void PlayShake(Vector3 shakePos,float power, float shakeTime)
    {
        if (_isPlaying)
        {
            float shakeDistance = Vector3.Distance(transform.position, shakePos) / power;
            if (_startPower > shakeDistance)
            {
                _startPower = shakeDistance;
                _shakeDistance = shakeDistance;
                _shakeTimer = shakeTime;
            }
            return; 
        }       
        _isPlaying = true;
        StartCoroutine(ShakeImpl(shakePos, power,shakeTime));
    }
    private IEnumerator ShakeImpl(Vector3 shakeInstancePos,float power , float shakeTime)
    {
        float shakeDistance = Vector3.Distance(transform.position, shakeInstancePos) / power;
        if (shakeDistance <= 0) { shakeDistance = 1f; }
        _shakeDistance = shakeDistance;
        _startPower = shakeDistance;
        _shakeTimer = shakeTime;
        _shakePos = _startPos;
        while (_shakeTimer > 0)
        {
            float range = _shakeAxis.x * _shakeRange / (_shakeDistance / _shakeTimer);
            if (range > _maxRange)
            {
                range = _maxRange;
            }
            _shakePos.x += _shakeAxis.x * range * (1 - 2 * Random.Range(0, 2));
            _shakePos.y += _shakeAxis.y * range * (1 - 2 * Random.Range(0, 2));
            _shakePos.z += _shakeAxis.z * range * (1 - 2 * Random.Range(0, 2));           
            transform.localPosition = _shakePos;
            _shakePos = _startPos;
            _shakeTimer -= Time.fixedDeltaTime;
            yield return _waitForFixedUpdate;
        }
        transform.localPosition = _startPos;
        _isPlaying = false;
    }
}
