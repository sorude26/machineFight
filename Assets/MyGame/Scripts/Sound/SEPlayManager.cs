using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SEの再生管理クラス
/// </summary>
public class SEPlayManager : MonoBehaviour
{
    /// <summary>
    /// SE情報クラス
    /// </summary>
    private class ClipInfo
    {
        /// <summary>音源名</summary>
        public string name;
        /// <summary>音源</summary>
        public AudioClip audioClip;
        /// <summary>音量</summary>
        public float volume;
        /// <summary>AudioMixerGroup</summary>
        public SoundManager.AudioMixerGroup audioMixerGroup;
        /// <summary>再生位置</summary>
        public Vector3 position;
        /// <summary>再生フラグ(true:再生済み/false:未再生)</summary>
        public bool isPlay;
        /// <summary>経過フレーム数</summary>
        public float frameCount;
    }
    /// <summary>再生遅延フレーム数</summary>
    [SerializeField, Range(1, 60)]
    private int _delayFrameCount = 2;
    /// <summary>再生登録上限</summary>
    [SerializeField, Range(1, 32)]
    private int _registrationMax = 4;
    /// <summary>立体音響SE再生リスト</summary>
    private readonly Dictionary<string, Queue<ClipInfo>> _playListWithVecter3 = new Dictionary<string, Queue<ClipInfo>>();
    /// <summary>再生オブジェクト管理クラス</summary>
    [SerializeField]
    private SoundObjectManager _soundObjectManager;

    void Update()
    {
        foreach (var queue in _playListWithVecter3.Values)
        {
            if (queue.Count == 0) continue;

            while (true)
            {
                if (queue.Count == 0) break;

                if (!queue.Peek().isPlay) break;

                var _ = queue.Dequeue();
            }

            if (queue.Count == 0) continue;

            var info = queue.Peek();
            info.frameCount++;
            if (info.frameCount > this._delayFrameCount)
            {
                SoundPlayer soundPlayer = _soundObjectManager.GetSound3DPlayer();
                soundPlayer?.gameObject.SetActive(true);
                soundPlayer.transform.position = info.position;
                soundPlayer.PlaySE(info.audioClip, info.volume, info.audioMixerGroup);
                var _ = queue.Dequeue();
            }
        }

        if (GetCount() == 0)
        {
            _playListWithVecter3.Clear();
            enabled = false;
        }
    }

    /// <summary>
    /// 効果音再生
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    /// <param name="audioMixerGroup"></param>
    /// <param name="pos"></param>
    public void PlaySE(AudioClip audioClip, float volume, SoundManager.AudioMixerGroup audioMixerGroup, Vector3 pos)
    {
        enabled = true;

        ClipInfo info = new ClipInfo
        {
            name = audioClip.name,
            audioClip = audioClip,
            volume = volume,
            audioMixerGroup = audioMixerGroup,
            position = pos,
            isPlay = false,
            frameCount = audioClip.length,
        };

        if (!_playListWithVecter3.ContainsKey(info.name))
        {
            // Queueが無かった時、SEを再生しQueueを作成する
            SoundPlayer soundPlayer = _soundObjectManager.GetSound3DPlayer();
            soundPlayer?.gameObject.SetActive(true);
            soundPlayer.transform.position = info.position;
            soundPlayer.PlaySE(info.audioClip, info.volume, info.audioMixerGroup);
            info.isPlay = true;

            var q = new Queue<ClipInfo>();
            q.Enqueue(info);
            _playListWithVecter3[info.name] = q;

            return;
        }
        
        // Queueに再生登録する
        Queue<ClipInfo> playList = _playListWithVecter3[info.name];
        if (playList.Count <= this._registrationMax)
        {
            _playListWithVecter3[info.name].Enqueue(info);
        }
        else
        {
            //Debug.Log("効果音の最大登録数を超えています。" + info.name);
        }
    }

    /// <summary>
    /// 要素数の取得
    /// </summary>
    private int GetCount()
    {
        int num = 0;
        foreach (var list in this._playListWithVecter3.Values)
        {
            num += list.Count;
        }
        return num;
    }
}
