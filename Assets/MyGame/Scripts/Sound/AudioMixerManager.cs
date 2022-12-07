using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>オーディオミキサーマネージャー</summary>
public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager Instance { get; private set; }
    [SerializeField]
    private AudioMixer _audioMixer = default;
    [SerializeField]
    private AudioMixerGroup[] _audioMixerGroup = default;
    /// <summary>デシベルの最小値</summary>
    private const float MIN_DECIBEL = -80f;
    /// <summary>デシベルの最大値</summary>
    private const float MAX_DECIBEL = 0f;
    /// <summary>マスターグループ名</summary>
    private const string MASTER_NAME = "MasterVolume";
    /// <summary>BGMグループ名</summary>
    private const string BGM_NAME = "BGMVolume";
    /// <summary>SEグループ名</summary>
    private const string SE_NAME = "SEVolume";

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// マスターの音量設定
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat(MASTER_NAME, DecibelConversion(volume));
    }

    /// <summary>
    /// BGMの音量設定
    /// </summary>
    /// <param name="volume"></param>
    public void SetBgmVolume(float volume)
    {
        _audioMixer.SetFloat(BGM_NAME, DecibelConversion(volume));
    }

    /// <summary>
    /// SEの音量設定
    /// </summary>
    /// <param name="volume"></param>
    public void SetSeVolume(float volume)
    {
        _audioMixer.SetFloat(SE_NAME, DecibelConversion(volume));
    }

    /// <summary>
    /// デシベル変換
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    private float DecibelConversion(float volume)
    {
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f, MIN_DECIBEL, MAX_DECIBEL);
    }

    /// <summary>
    /// ミキサーグループを取得する
    /// </summary>
    /// <param name="mixerGroup">取得したいグループ</param>
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
                Debug.LogWarning("指定されたAudioMixerGroupがありません");
                return null;
        }
    }
}
