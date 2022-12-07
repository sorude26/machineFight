using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>AudioSource拡張クラス</summary> 
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
    /// 再生終了時のコールバック付き再生機能
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
