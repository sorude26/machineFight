using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�T�E���h�e�X�g�p�N���X</summary>
public class SoundTester : MonoBehaviour
{
    [SerializeField]
    GameObject _right;
    [SerializeField]
    GameObject _left;
    [SerializeField]
    Material _defaultMaterial;
    [SerializeField]
    Material _activeMaterial;
    MeshRenderer _rightRenderer;
    MeshRenderer _leftRenderer;

    [SerializeField]
    bool _isPlayBGM = false;
    [SerializeField]
    int _playBGMIndex = 0;
    [SerializeField]
    int _playRightIndex = 0;
    [SerializeField]
    int _playLeftIndex = 0;
    [SerializeField]
    int _playCount = 1;

    void Start()
    {
        _rightRenderer = _right.GetComponent<MeshRenderer>();
        _leftRenderer = _left.GetComponent<MeshRenderer>();
        
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(2.0f);

        if (_isPlayBGM)
        {
            Debug.Log(_playBGMIndex + "�Ԃ�BGM���Đ����܂�");
            SoundManager.Instance.PlayBGM(_playBGMIndex, 0.5f);
        }
        
        while (true)
        {
            yield return new WaitForSeconds(2.0f);

            //-------------�����p-------------------//
            _rightRenderer.material = _activeMaterial;
            _leftRenderer.material = _defaultMaterial;
            //----------------------------------------//

            Debug.Log(_playRightIndex + "�Ԃ�" + _playCount + "��Đ����܂�");
            StartCoroutine(TestPlay(_playRightIndex, _right.transform.position));
            
            yield return new WaitForSeconds(2.0f);
            
            //-------------�����p-------------------//
            _rightRenderer.material = _defaultMaterial;
            _leftRenderer.material = _activeMaterial;
            //----------------------------------------//

            Debug.Log(_playLeftIndex + "�Ԃ�" + _playCount + "��Đ����܂�");
            StartCoroutine(TestPlay(_playLeftIndex, _left.transform.position));
        }
    }

    IEnumerator TestPlay(int index, Vector3 pos)
    {
        for(int i = 0; i < _playCount; i++)
        {
            SoundManager.Instance.PlaySE(index, pos);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }
}
