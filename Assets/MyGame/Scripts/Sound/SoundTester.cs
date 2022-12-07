using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>サウンドテスト用クラス</summary>
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
            Debug.Log(_playBGMIndex + "番のBGMを再生します");
            SoundManager.Instance.PlayBGM(_playBGMIndex, 0.5f);
        }
        
        while (true)
        {
            yield return new WaitForSeconds(2.0f);

            //-------------可視化用-------------------//
            _rightRenderer.material = _activeMaterial;
            _leftRenderer.material = _defaultMaterial;
            //----------------------------------------//

            Debug.Log(_playRightIndex + "番を" + _playCount + "回再生します");
            StartCoroutine(TestPlay(_playRightIndex, _right.transform.position));
            
            yield return new WaitForSeconds(2.0f);
            
            //-------------可視化用-------------------//
            _rightRenderer.material = _defaultMaterial;
            _leftRenderer.material = _activeMaterial;
            //----------------------------------------//

            Debug.Log(_playLeftIndex + "番を" + _playCount + "回再生します");
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
