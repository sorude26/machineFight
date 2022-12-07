using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>サウンドマネージャークラス</summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    /// <summary>サウンドリスト</summary>
    [SerializeField]
    private SoundClipList _soundList;
    /// <summary>通常サウンド再生機能</summary>
    [SerializeField]
    private SoundPlayer _soundPlayer;
    /// <summary>立体音響サウンド再生機能</summary>
    [SerializeField]
    private SoundPlayer _sound3DPlayer;
    /// <summary>BGM用のAudioSource</summary>
    [SerializeField]
    private AudioSource _bgmAudioSource;

    public enum AudioMixerGroup
    {
        BGM,
        SE,
        SYSTEM_SE,
        PLAYER_SE,
        ENEMY_SE,
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="volume"></param>
    public void PlayBGM(int soundId, float volume = 1f)
    {
        AudioClip audioClip = _soundList.GetAudioClip(soundId);
        if (audioClip == null) return;

        _bgmAudioSource.Play(audioClip, volume);
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="volume"></param>
    /// <param name="mixerGroup"></param>
    public void PlaySE(int soundId, float volume = 1f, AudioMixerGroup mixerGroup = AudioMixerGroup.SE)
    {
        AudioClip audioClip = _soundList.GetAudioClip(soundId);
        if (audioClip == null) return;

        SoundPlayer soundPlayer = GetSoundPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.PlaySE(audioClip, volume, mixerGroup);
    }

    /// <summary>
    /// 立体音響でSEを再生する
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="pos"></param>
    /// <param name="volume"></param>
    /// <param name="mixerGroup"></param>
    public void PlaySE(int soundId, Vector3 pos, float volume = 1f, AudioMixerGroup mixerGroup = AudioMixerGroup.SE)
    {
        AudioClip audioClip = _soundList.GetAudioClip(soundId);
        if (audioClip == null) return;

        SoundPlayer soundPlayer = GetSound3DPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.transform.position = pos;
        soundPlayer.PlaySE(audioClip, volume, mixerGroup);
    }


    /// <summary>
    /// プールからSoundPlayerを取得する
    /// </summary>
    /// <returns>SoundPlayer</returns>
    private SoundPlayer GetSoundPlayer()
    {
        return AudioSourcePool.GetObject(_soundPlayer);
    }

    /// <summary>
    /// プールから3D用のSoundPlayerを取得する
    /// </summary>
    /// <returns>SoundPlayer for 3D</returns>
    private SoundPlayer GetSound3DPlayer()
    {
        return AudioSource3DPool.GetObject(_sound3DPlayer);
    }
}
