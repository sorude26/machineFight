using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�T�E���h���X�g�N���X</summary>
public class SoundClipList : MonoBehaviour
{
    /// <summary>�T�E���h�N���b�v</summary>
    [SerializeField]
    private List<AudioClip> audioClips = new List<AudioClip>();

    /// <summary>
    /// ���X�g����N���b�v���擾����
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(int index)
    {
        if(audioClips[index] == null)
        {
            Debug.LogWarning("�w�肳�ꂽ�ԍ��ɃT�E���h���o�^����Ă��܂���ł����B �w��ԍ� : " + index);
            return null;
        }
        return audioClips[index];
    }
}
