using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatcher : MonoBehaviour
{
    [SerializeField]
    private PlayerMachineController _playerMachine = default;
    [SerializeField]
    private ParticleSystem _catchEffect = default;

    public PlayerMachineController Player { get => _playerMachine; }
    public void PlayCatchEffect()
    {
        _catchEffect?.Play();
    }
}