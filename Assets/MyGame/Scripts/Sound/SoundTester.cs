using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>サウンドテスター</summary>
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
    [SerializeField]
    MeshRenderer _rightRenderer;
    [SerializeField]
    MeshRenderer _leftRenderer;

    void Start()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(3.0f);

        SoundManager.Instance.PlayBGM(0, 0.5f);

        while (true)
        {
            yield return new WaitForSeconds(3.0f);

            //-------------可視化用-------------------//
            _rightRenderer.material = _activeMaterial;
            _leftRenderer.material = _defaultMaterial;
            //----------------------------------------//

            SoundManager.Instance.Play3DSE(1, _right.transform.position);
            
            yield return new WaitForSeconds(3.0f);
            
            //-------------可視化用-------------------//
            _rightRenderer.material = _defaultMaterial;
            _leftRenderer.material = _activeMaterial;
            //----------------------------------------//

            SoundManager.Instance.Play3DSE(2, _left.transform.position);
        }
    }
}
