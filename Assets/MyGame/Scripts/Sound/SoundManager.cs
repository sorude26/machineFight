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

    public enum SoundMixerGroup
    {
        BGM,
        SE,
        SYSTEM_SE,
        PLAYER_SE,
        ENEMY_SE,
    }

    private void Awake()
    {
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
        _bgmAudioSource.Play(_soundList.GetAudioClip(soundId), volume);
    }

    /// <summary>
    /// SE���Đ�����
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="volume"></param>
    /// <param name="mixerGroup"></param>
    public void PlaySE(int soundId, float volume = 1f, SoundMixerGroup mixerGroup = SoundMixerGroup.SE)
    {
        SoundPlayer soundPlayer = GetSoundPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.PlaySE(_soundList.GetAudioClip(soundId), volume, mixerGroup);
    }

    /// <summary>
    /// ���̉�����SE���Đ�����
    /// </summary>
    /// <param name="soundId"></param>
    /// <param name="pos"></param>
    /// <param name="volume"></param>
    /// <param name="mixerGroup"></param>
    public void Play3DSE(int soundId, Vector3 pos, float volume = 1f, SoundMixerGroup mixerGroup = SoundMixerGroup.SE)
    {
        SoundPlayer soundPlayer = GetSound3DPlayer();
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.transform.position = pos;
        soundPlayer.PlaySE(_soundList.GetAudioClip(soundId), volume, mixerGroup);
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
