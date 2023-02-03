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
    /// <summary>BGM�p��AudioSource</summary>
    [SerializeField]
    private AudioSource _bgmAudioSource;
    /// <summary>AudioSource</summary>
    [SerializeField]
    private AudioSource _deactiveAudioSource;
    /// <summary>�T�E���h�Đ��L������</summary>
    [SerializeField]
    float _playSoundDistance = 100f;
    /// <summary>�Đ��I�u�W�F�N�g�����N���X</summary>
    [SerializeField]
    private SoundObjectManager _soundObjectManager;
    /// <summary>SE�Đ��Ǘ��N���X</summary>
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
    /// BGM���N���X�t�F�[�h�Đ�����
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
    /// SE���Đ�����
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
        
        if (!DistanceCheck(pos)) return;

        _sePlayManager.PlaySE(audioClip, volume, mixerGroup, pos);
    }
    
    /// <summary>
    /// �^�[�Q�b�g�ɒǏ]���Ȃ���SE���Đ�����
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
<<<<<<< HEAD
    /// ���[�v����SE���Đ�����(���̉����ł͂Ȃ�)
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

    /// <summary>
=======
>>>>>>> bef1902390a7f0822a1c2d9270f3b4e0ae4f815f
    /// �T�E���h�̍Đ���������
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
