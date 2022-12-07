using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>�T�E���h�Đ��N���X</summary>
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;
    private UnityEvent _callback = new UnityEvent();

    private void Awake()
    {
        _callback.AddListener(() => DisActive(gameObject));
    }

    /// <summary>
    /// BGM�Đ�
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlayBGM(AudioClip audioClip, float volume = 1f)
    {
        _audioSource.loop = true;
        _audioSource.outputAudioMixerGroup = AudioMixerManager.Instance.GetAudioMixerGroup(SoundManager.AudioMixerGroup.BGM);
        _audioSource.Play(audioClip, volume);
    }

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    /// <param name="group"></param>
    public void PlaySE(AudioClip audioClip, float volume = 1f, SoundManager.AudioMixerGroup group = SoundManager.AudioMixerGroup.SE)
    {
        _audioSource.loop = false;
        _audioSource.outputAudioMixerGroup = AudioMixerManager.Instance.GetAudioMixerGroup(group);
        StartCoroutine(_audioSource.PlayWithCompCallback(audioClip, volume, _callback));
    }

    /// <summary>
    /// �Đ��I���㎩�g���A�N�e�B�u������
    /// Event�ɓo�^���Ă���
    /// </summary>
    /// <param name="gameObject"></param>
    private void DisActive(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
