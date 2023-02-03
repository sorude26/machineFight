using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SE�̍Đ��Ǘ��N���X
/// </summary>
public class SEPlayManager : MonoBehaviour
{
    /// <summary>
    /// SE���N���X
    /// </summary>
    private class ClipInfo
    {
        /// <summary>������</summary>
        public string name;
        /// <summary>����</summary>
        public AudioClip audioClip;
        /// <summary>����</summary>
        public float volume;
        /// <summary>AudioMixerGroup</summary>
        public SoundManager.AudioMixerGroup audioMixerGroup;
        /// <summary>�Đ��ʒu</summary>
        public Vector3 position;
        /// <summary>�Đ��t���O(true:�Đ��ς�/false:���Đ�)</summary>
        public bool isPlay;
        /// <summary>�o�߃t���[����</summary>
        public float frameCount;
    }
    /// <summary>�Đ��x���t���[����</summary>
    [SerializeField, Range(1, 60)]
    private int _delayFrameCount = 2;
    /// <summary>�Đ��o�^���</summary>
    [SerializeField, Range(1, 32)]
    private int _registrationMax = 4;
    /// <summary>���̉���SE�Đ����X�g</summary>
    private readonly Dictionary<string, Queue<ClipInfo>> _playListWithVecter3 = new Dictionary<string, Queue<ClipInfo>>();
    /// <summary>�Đ��I�u�W�F�N�g�Ǘ��N���X</summary>
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
    /// ���ʉ��Đ�
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
            // Queue�������������ASE���Đ���Queue���쐬����
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
        
        // Queue�ɍĐ��o�^����
        Queue<ClipInfo> playList = _playListWithVecter3[info.name];
        if (playList.Count <= this._registrationMax)
        {
            _playListWithVecter3[info.name].Enqueue(info);
        }
        else
        {
            //Debug.Log("���ʉ��̍ő�o�^���𒴂��Ă��܂��B" + info.name);
        }
    }

    /// <summary>
    /// �v�f���̎擾
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
