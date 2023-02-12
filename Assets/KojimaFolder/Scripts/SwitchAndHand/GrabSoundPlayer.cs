using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSoundPlayer : MonoBehaviour
{
    [SerializeField]
    Switch _switch;

    [SerializeField]
    int _grabinSoundId;
    [SerializeField]
    float _grabinVolume;
    [SerializeField]
    int _graboutSoundId;
    [SerializeField]
    float _graboutVolume;

    private IEnumerator Start()
    {
        //3ƒtƒŒ[ƒ€‘Ò‚Â
        for (int i = 0; i < 3; i++)
        {
            yield return null;
        }
        _switch.OnLockIn += PlayGrabinSound;
        _switch.OnFree += PlayGraboutSound;
    }

    void PlayGrabinSound()
    {
        SoundManager.Instance.PlaySE(_grabinSoundId, this.gameObject, _grabinVolume);
    }

    void PlayGraboutSound()
    {
        SoundManager.Instance.PlaySE(_graboutSoundId, this.gameObject, _graboutVolume);
    }
}
