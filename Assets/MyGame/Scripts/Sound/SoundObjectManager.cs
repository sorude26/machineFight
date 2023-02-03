using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>再生オブジェクト供給クラス</summary>
public class SoundObjectManager : MonoBehaviour
{
    /// <summary>通常サウンド再生機能</summary>
    [SerializeField]
    private SoundPlayer _soundPlayer;
    /// <summary>立体音響サウンド再生機能</summary>
    [SerializeField]
    private SoundPlayer _sound3DPlayer;

    /// <summary>
    /// プールからSoundPlayerを取得する
    /// </summary>
    /// <returns>SoundPlayer</returns>
    public SoundPlayer GetSoundPlayer()
    {
        return AudioSourcePool.GetObject(_soundPlayer);
    }

    /// <summary>
    /// プールから3D用のSoundPlayerを取得する
    /// </summary>
    /// <returns>SoundPlayer for 3D</returns>
    public SoundPlayer GetSound3DPlayer()
    {
        return AudioSource3DPool.GetObject(_sound3DPlayer);
    }
}
