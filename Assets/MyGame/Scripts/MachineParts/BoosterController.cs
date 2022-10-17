using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _mainBoosters = default;
    [SerializeField]
    private ParticleSystem[] _jetBoosters = default;
    [SerializeField]
    private ParticleSystem[] _backBoosters = default;
    [SerializeField]
    private ParticleSystem[] _leftBoosters = default;
    [SerializeField]
    private ParticleSystem[] _rightBoosters = default;
    public bool IsBoost { get; private set; }
    public void StartBooster()
    {
        foreach (var booster in _mainBoosters)
        {
            booster.Play();
        }
        IsBoost = true;
    }
    public void StopBooster()
    {
        foreach (var booster in _mainBoosters)
        {
            booster.Stop();
        }
        IsBoost = false;
    }
    public void MainBoost()
    {
        foreach (var booster in _jetBoosters)
        {
            booster.Play();
        }
        LeftBoost();
        RightBoost();
    }
    public void BackBoost()
    {
        foreach (var booster in _backBoosters)
        {
            booster.Play();
        }
        LeftBoost();
        RightBoost();
    }
    public void LeftBoost()
    {
        foreach (var booster in _leftBoosters)
        {
            booster.Play();
        }
    }
    public void RightBoost()
    {
        foreach (var booster in _rightBoosters)
        {
            booster.Play();
        }
    }
}
