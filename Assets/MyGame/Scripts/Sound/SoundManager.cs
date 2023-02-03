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
    /// <summary>BGM用のAudioSource</summary>
    [SerializeField]
    private AudioSource _bgmAudioSource;
    /// <summary>AudioSource</summary>
    [SerializeField]
    private AudioSource _deactiveAudioSource;
    /// <summary>サウンド再生有効距離</summary>
    [SerializeField]
    float _playSoundDistance = 100f;
    /// <summary>再生オブジェクト供給クラス</summary>
    [SerializeField]
    private SoundObjectManager _soundObjectManager;
    /// <summary>SE再生管理クラス</summary>
    [SerializeField]
    private SEPlayManager _sePlayManager;
    
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

        SoundPlayer soundPlayer = _soundObjectManager.GetSoundPlayer();
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

        _sePlayManager.PlaySE(audioClip, volume, mixerGroup, pos);
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

        SoundPlayer soundPlayer = _soundObjectManager.GetSound3DPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.PlaySE(audioClip, target, volume, mixerGroup);
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
