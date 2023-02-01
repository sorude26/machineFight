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
    /// <summary>AudioSource</summary>
    [SerializeField]
    private AudioSource _deactiveAudioSource;
    /// <summary>サウンド再生有効距離</summary>
    [SerializeField]
    float _playSoundDistance = 100f;

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
    /// BGMをクロスフェード再生する
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="fadeTime"></param>
    /// <param name="volume"></param>
    public void PlayBGMWithCrossFade(int soundId, float fadeTime, float volume = 1f)
    {
        AudioClip audioClip = _soundList.GetAudioClip(soundId);
        if (audioClip == null) return;

        StartCoroutine(_bgmAudioSource.StopWithFadeOut(fadeTime));
        StartCoroutine(_deactiveAudioSource.PlayWithFadeIn(audioClip, fadeTime, volume));
        (_bgmAudioSource, _deactiveAudioSource) = (_deactiveAudioSource, _bgmAudioSource);
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
        
        if (!DistanceCheck(pos)) return;
        
        SoundPlayer soundPlayer = GetSound3DPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.transform.position = pos;
        soundPlayer.PlaySE(audioClip, volume, mixerGroup);
    }
    
    /// <summary>
    /// ターゲットに追従しながらSEを再生する
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="target"></param>
    /// <param name="volume"></param>
    /// <param name="mixerGroup"></param>
    public void PlaySE(int soundId, GameObject target, float volume = 1f, AudioMixerGroup mixerGroup = AudioMixerGroup.SE)
    {
        AudioClip audioClip = _soundList.GetAudioClip(soundId);
        if (audioClip == null) return;

        SoundPlayer soundPlayer = GetSound3DPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.PlaySE(audioClip, target, volume, mixerGroup);
    }

    /// <summary>
    /// ループするSEを再生する(立体音響ではない)
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="target"></param>
    /// <param name="volume"></param>
    /// <param name="mixerGroup"></param>
    /// <returns></returns>
    public SoundPlayer PlaySELoop(int soundId, GameObject target, float volume = 1f, AudioMixerGroup mixerGroup = AudioMixerGroup.SE)
    {
        AudioClip audioClip = _soundList.GetAudioClip(soundId);
        if (audioClip == null) return null;

        SoundPlayer soundPlayer = GetSoundPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.PlayLoopSE(audioClip, target, volume, mixerGroup);
        return soundPlayer;
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

    /// <summary>
    /// サウンドの再生距離判定
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    private bool DistanceCheck(Vector3 targetPos)
    {
        float dis = Vector3.Distance(NavigationManager.Instance.Target.transform.position, targetPos);
        if (dis > _playSoundDistance)
        {
            return false;
        }
        return true;
    }
}
