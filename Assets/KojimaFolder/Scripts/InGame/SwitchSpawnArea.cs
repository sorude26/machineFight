using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSpawnArea : MonoBehaviour
{
    [SerializeField]
    Transform _leftUp;
    [SerializeField]
    Transform _rightDown;

    Transform[] _positions = new Transform[0];

    public IEnumerable<Transform> Positions => _positions;

    /// <summary>
    /// 縦、横の量を指定してエリア内に生成ポイントを作成する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public IEnumerable<Transform> MakeSpawnPositions(int x, int y)
    {
        foreach (Transform t in _positions)
        {
            if (t != null)
            {
                Destroy(t.gameObject);
            }
        }
        _positions = new Transform[x * y];
        float rightLength = (_rightDown.localPosition.x - _leftUp.localPosition.x) / (x + 1);
        float downLength = (_rightDown.localPosition.y - _leftUp.localPosition.y) / (y + 1);
        float posY = _leftUp.localPosition.y;
        int index = 0;
        for (int iy = 0; iy < y; iy++)
        {
            posY += downLength;
            float posX = _leftUp.localPosition.x;
            for (int ix = 0; ix < x; ix++)
            {
                posX += rightLength;
                Transform newPos = new GameObject($"position[{index}]").transform;
                newPos.parent = this.transform;
                newPos.localPosition = new Vector3(posX, posY,0);
                newPos.localRotation = Quaternion.identity;
                _positions[index] = newPos;
                ++index;
            }
        }
        return _positions;
    }

    private void OnDrawGizmos()
    {
        var pos = _leftUp.localPosition;
        pos.z = 0;
        _leftUp.localPosition = pos;
        pos = _rightDown.localPosition;
        pos.z = 0;
        _rightDown.localPosition = pos;

        float sizeX = (_rightDown.localPosition.x - _leftUp.localPosition.x);
        float sizeY = (_rightDown.localPosition.y - _leftUp.localPosition.y);
        Vector3 rightUp = _leftUp.position + _leftUp.right * sizeX;
        Vector3 leftDown = _leftUp.position + _leftUp.up * sizeY;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_leftUp.position, rightUp);
        Gizmos.DrawLine(_rightDown.position, leftDown);
        Gizmos.DrawLine(_leftUp.position, leftDown);
        Gizmos.DrawLine(_rightDown.position, rightUp);
    }
}
