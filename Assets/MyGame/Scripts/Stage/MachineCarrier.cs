using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineCarrier : MonoBehaviour
{
    [SerializeField]
    private PlayerMachineController _controller = default;
    [SerializeField]
    private Transform _machineTrans = default;
    [SerializeField]
    private Transform _machineBaseTrans = default;
    [SerializeField]
    private Transform _baseTrans = default;
    [SerializeField]
    private ParticleSystem[] _effects = default;
    private bool _isCarrier = true;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _machineTrans.SetParent(_baseTrans);
        _machineTrans.localPosition = Vector3.zero;
        _animator.speed = 0;
    }
    private void LateUpdate()
    {
        if(_isCarrier == false)
        {
            return;
        }
        if(!PlayerVrCockpit.Instance.gameObject.activeInHierarchy || MannedOperationSystem.Instance.IsOnline)
        {
            //VRモードでは起動完了までキャリアーを動かさない
            _animator.speed = 1;
        }
        _machineTrans.localPosition = Vector3.zero;
    }
    public void PlayEffect()
    {
        foreach (var effect in _effects)
        {
            effect.Play();
        }
    }
    public void CarrierEnd()
    {
        StartCoroutine(Carrier());
        _controller.EndWaitMode();
        _isCarrier = false;
        _machineTrans.SetParent(_machineBaseTrans);
    }
    private IEnumerator Carrier()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
