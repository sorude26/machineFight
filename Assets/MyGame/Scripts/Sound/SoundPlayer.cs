using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>サウンド再生クラス</summary>
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;
    private UnityEvent _callback = new UnityEvent();

    private void Awake()
    {
        _callback.AddListener(() => Deactivate(gameObject));
    }

    /// <summary>
    /// BGM再生
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
    /// SE再生
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
    /// ターゲットに追従しながらSEを再生する
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="target"></param>
    /// <param name="volume"></param>
    /// <param name="group"></param>
    public void PlaySE(AudioClip audioClip, GameObject target, float volume = 1f, SoundManager.AudioMixerGroup group = SoundManager.AudioMixerGroup.SE)
    {
        _audioSource.loop = false;
        _audioSource.outputAudioMixerGroup = AudioMixerManager.Instance.GetAudioMixerGroup(group);
        StartCoroutine(FollowTarget(target));
        StartCoroutine(_audioSource.PlayWithCompCallback(audioClip, volume, _callback));
    }

    /// <summary>
    /// ターゲットがアクティブの間追従する
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator FollowTarget(GameObject target)
    {
        while(true)
        {
            if (!target.activeSelf)
            {
                Deactivate(gameObject);
            }
            transform.position = target.transform.position;
            yield return null;
        }
    }

    /// <summary>
    /// 再生終了後自身を非アクティブ化する
    /// Eventに登録しておく
    /// </summary>
    /// <param name="gameObject"></param>
    private void Deactivate(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
