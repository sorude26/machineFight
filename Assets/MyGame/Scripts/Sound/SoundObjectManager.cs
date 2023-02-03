using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�Đ��I�u�W�F�N�g�����N���X</summary>
public class SoundObjectManager : MonoBehaviour
{
    /// <summary>�ʏ�T�E���h�Đ��@�\</summary>
    [SerializeField]
    private SoundPlayer _soundPlayer;
    /// <summary>���̉����T�E���h�Đ��@�\</summary>
    [SerializeField]
    private SoundPlayer _sound3DPlayer;

    /// <summary>
    /// �v�[������SoundPlayer���擾����
    /// </summary>
    /// <returns>SoundPlayer</returns>
    public SoundPlayer GetSoundPlayer()
    {
        return AudioSourcePool.GetObject(_soundPlayer);
    }

    /// <summary>
    /// �v�[������3D�p��SoundPlayer���擾����
    /// </summary>
    /// <returns>SoundPlayer for 3D</returns>
    public SoundPlayer GetSound3DPlayer()
    {
        return AudioSource3DPool.GetObject(_sound3DPlayer);
    }
}
