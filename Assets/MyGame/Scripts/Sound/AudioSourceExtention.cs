using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>AudioSource�g���N���X</summary> 
public static class AudioSourceExtention
{
    public static void Play(this AudioSource audioSource, AudioClip audioClip, float volume = 1f)
    {
        if (audioClip == null) return;

        audioSource.clip = audioClip;
        audioSource.volume = volume;

        audioSource.Play();
    }

    /// <summary>
    /// �Đ��I�����̃R�[���o�b�N�t���Đ��@�\
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    /// <param name="compCallback"></param>
    /// <returns></returns>
    public static IEnumerator PlayWithCompCallback(this AudioSource audioSource, AudioClip audioClip, float volume = 1f, UnityEvent compCallback = null)
    {
        audioSource.Play(audioClip, volume);

        yield return new WaitForSeconds(audioClip.length);

        compCallback?.Invoke();
        
        yield break;
    }
}
