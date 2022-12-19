using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateWall : MonoBehaviour
{
    [SerializeField]
    private GameObject _wall = default;
    [SerializeField]
    private GameObject _gateWll = default;
    [SerializeField]
    private Animator _gate = default;
    [SerializeField]
    private string _openName = "OpenGate";
    [SerializeField]
    private string _closeName = "CloseGate";
    public void SetGateWall(bool isGate)
    {
        _wall.SetActive(!isGate);
        _gateWll.SetActive(isGate);
    }
    public void OpenGate()
    {
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
        _gate.Play(_openName);
    }
    public void CloseGate()
    {

        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
        _gate.Play(_closeName);
    }
}
