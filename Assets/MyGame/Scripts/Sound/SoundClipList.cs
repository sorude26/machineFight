using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>サウンドリストクラス</summary>
public class SoundClipList : MonoBehaviour
{
    /// <summary>サウンドクリップ</summary>
    [SerializeField]
    private List<AudioClip> audioClips = new List<AudioClip>();

    /// <summary>
    /// リストからクリップを取得する
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(int index)
    {
        if(audioClips[index] == null)
        {
            Debug.LogWarning("指定された番号にサウンドが登録されていませんでした。 指定番号 : " + index);
            return null;
        }
        return audioClips[index];
    }
}
