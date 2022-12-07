using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>�I�[�f�B�I�~�L�T�[�}�l�[�W���[</summary>
public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager Instance { get; private set; }
    [SerializeField]
    private AudioMixer _audioMixer = default;
    [SerializeField]
    private AudioMixerGroup[] _audioMixerGroup = default;
    /// <summary>�f�V�x���̍ŏ��l</summary>
    private const float MIN_DECIBEL = -80f;
    /// <summary>�f�V�x���̍ő�l</summary>
    private const float MAX_DECIBEL = 0f;
    /// <summary>�}�X�^�[�O���[�v��</summary>
    private const string MASTER_NAME = "MasterVolume";
    /// <summary>BGM�O���[�v��</summary>
    private const string BGM_NAME = "BGMVolume";
    /// <summary>SE�O���[�v��</summary>
    private const string SE_NAME = "SEVolume";

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// �}�X�^�[�̉��ʐݒ�
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat(MASTER_NAME, DecibelConversion(volume));
    }

    /// <summary>
    /// BGM�̉��ʐݒ�
    /// </summary>
    /// <param name="volume"></param>
    public void SetBgmVolume(float volume)
    {
        _audioMixer.SetFloat(BGM_NAME, DecibelConversion(volume));
    }

    /// <summary>
    /// SE�̉��ʐݒ�
    /// </summary>
    /// <param name="volume"></param>
    public void SetSeVolume(float volume)
    {
        _audioMixer.SetFloat(SE_NAME, DecibelConversion(volume));
    }

    /// <summary>
    /// �f�V�x���ϊ�
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    private float DecibelConversion(float volume)
    {
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f, MIN_DECIBEL, MAX_DECIBEL);
    }

    /// <summary>
    /// �~�L�T�[�O���[�v���擾����
    /// </summary>
    /// <param name="mixerGroup">�擾�������O���[�v</param>
    /// <returns></returns>
    public AudioMixerGroup GetAudioMixerGroup(SoundManager.AudioMixerGroup mixerGroup)
    {
        switch (mixerGroup)
        {
            case SoundManager.AudioMixerGroup.BGM:
                return _audioMixerGroup[0];
            case SoundManager.AudioMixerGroup.SE:
                return _audioMixerGroup[1];
            case SoundManager.AudioMixerGroup.SYSTEM_SE:
                return _audioMixerGroup[2];
            case SoundManager.AudioMixerGroup.PLAYER_SE:
                return _audioMixerGroup[3];
            case SoundManager.AudioMixerGroup.ENEMY_SE:
                return _audioMixerGroup[4];
            default:
                Debug.LogWarning("�w�肳�ꂽAudioMixerGroup������܂���");
                return null;
        }
    }
}
