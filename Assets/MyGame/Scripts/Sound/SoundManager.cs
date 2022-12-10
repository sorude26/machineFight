using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�T�E���h�}�l�[�W���[�N���X</summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    /// <summary>�T�E���h���X�g</summary>
    [SerializeField]
    private SoundClipList _soundList;
    /// <summary>�ʏ�T�E���h�Đ��@�\</summary>
    [SerializeField]
    private SoundPlayer _soundPlayer;
    /// <summary>���̉����T�E���h�Đ��@�\</summary>
    [SerializeField]
    private SoundPlayer _sound3DPlayer;
    /// <summary>BGM�p��AudioSource</summary>
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
    /// BGM���Đ�����
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
    /// SE���Đ�����
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
    /// ���̉�����SE���Đ�����
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
    /// �v�[������SoundPlayer���擾����
    /// </summary>
    /// <returns>SoundPlayer</returns>
    private SoundPlayer GetSoundPlayer()
    {
        return AudioSourcePool.GetObject(_soundPlayer);
    }

    /// <summary>
    /// �v�[������3D�p��SoundPlayer���擾����
    /// </summary>
    /// <returns>SoundPlayer for 3D</returns>
    private SoundPlayer GetSound3DPlayer()
    {
        return AudioSource3DPool.GetObject(_sound3DPlayer);
    }
}