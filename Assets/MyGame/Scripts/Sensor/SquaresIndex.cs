using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ꌳ�z��ł̃}�X�ڂ�index��������
/// </summary>
public class SquaresIndex 
{
    /// <summary> �������̍ő�index�� </summary>
    private int _maxHorizontal;
    /// <summary> �S�̂̍ő�index�� </summary>
    private int _maxIndex;
    public SquaresIndex(int horizontal, int vertical)
    {
        _maxHorizontal = horizontal;
        _maxIndex = horizontal * vertical;
    }    
    /// <summary>
    /// ���͔�������Index��Ԃ�
    /// </summary>
    /// <param name="index">�w��_</param>
    /// <returns></returns>
    public IEnumerable<int> GetNeighor(int index)
    {
        //�O�i
        if (index >= _maxHorizontal)
        {
            foreach (int neighor in GetLeftMiddleRight(index - _maxHorizontal))
            {
                yield return neighor;
            }
        }
        //���i
        if (index % _maxHorizontal > 0)
        {
            yield return index - 1;  //��
        }
        if (index % _maxHorizontal < _maxHorizontal - 1)
        {
            yield return index + 1;  //�E
        }
        yield return index;
        //��i
        if (index + _maxHorizontal < _maxIndex)
        {
            foreach (int neighor in GetLeftMiddleRight(index + _maxHorizontal))
            {
                yield return neighor;
            }
        }
    }
    /// <summary>
    /// ���͎l������Index��Ԃ�
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerable<int> GetNeighorCross(int index)
    {
        //�O�i
        if (index >= _maxHorizontal)
        {
            yield return index - _maxHorizontal;
        }
        //���i
        if (index % _maxHorizontal > 0)
        {
            yield return index - 1;  //��
        }
        if (index % _maxHorizontal < _maxHorizontal - 1)
        {
            yield return index + 1;  //�E
        }
        //��i
        if (index + _maxHorizontal < _maxIndex)
        {
            yield return index + _maxHorizontal;
        }
    }
    /// <summary>
    /// �w��_�ƍ��E��Index��Ԃ�
    /// </summary>
    /// <param name="index">�w��_</param>
    /// <returns></returns>
    public IEnumerable<int> GetLeftMiddleRight(int index)
    {
        if (index % _maxHorizontal > 0)
        {
            //��
            yield return index - 1;
        }
        //����
        yield return index;
        if (index % _maxHorizontal < _maxHorizontal - 1)
        {
            //�E
            yield return index + 1;
        }
    }
}
