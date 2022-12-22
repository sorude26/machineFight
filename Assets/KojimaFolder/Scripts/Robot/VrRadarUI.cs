using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VrRadarUI : MonoBehaviour
{
    const float CANVAS_WIDTH_HEIGHT = 256f;
    [SerializeField]
    Image _scanLine;
    [SerializeField]
    float _scanSpeed; //���b�ň��]���邩
    [SerializeField]
    float _scanRange;
    [SerializeField]
    Camera _camera;
    [SerializeField]
    VrRadarIconUI _iconPrefab;

    (LockOnTarget target, bool isAlradyShowed)[] _targets = new (LockOnTarget target, bool isAlradyShowed)[0];
    int _targetsLength;
    List<VrRadarIconUI> _icons = new List<VrRadarIconUI>();

    float _speed;
    Vector3 _scanRotate;


    private void Awake()
    {
        _speed = _scanSpeed / 365f;
    }

    private void FixedUpdate()
    {
        var newRotate = _scanRotate;
        //���v���
        newRotate.z -= Time.fixedDeltaTime / _speed;
        newRotate = newRotate.NomalizeRotate360();
        if (newRotate.z > _scanRotate.z)
        {
            RestTargetFlag();
        }
        _scanRotate = newRotate;
        UpdateRadar();
        _scanLine.rectTransform.localEulerAngles = _scanRotate;
        _camera.Render();
    }

    /// <summary>
    /// ���[�_�[�����]�����Ƃ��A���ׂẴ^�[�Q�b�g���܂��\������Ă��Ȃ���Ԃ̔���ɂȂ�B
    /// </summary>
    void RestTargetFlag()
    {
        int length = LockOnController.Instance.LockOnTargets.Count;
        if (length > _targets.Length)
        {
            _targets = new (LockOnTarget, bool)[length];
        }
        int i = 0;
        foreach (var target in LockOnController.Instance.LockOnTargets)
        {
            _targets[i] = (target, false);
            ++i;
        }
        _targetsLength = length;
    }

    void UpdateRadar()
    {
        for (int i = 0; i < _targetsLength; i++)
        {
            if (_targets[i].isAlradyShowed) continue;

            Vector3 diff = _targets[i].target.transform.position - PlayerVrCockpit.Instance.transform.position;
            diff.y = 0;
            Vector3 f = PlayerVrCockpit.Instance.transform.forward;
            f.y = 0;
            float angle = Vector3.Angle(f, diff);
            angle = Vector3.Cross(f, diff).y > 0 ? 360f - angle : angle;
            if (angle < _scanRotate.z || Mathf.Abs(angle - _scanRotate.z) > 45f) continue;

            _targets[i].isAlradyShowed = true;
            ShowIcon(_targets[i].target.transform.position);

        }
    }

    void ShowIcon(Vector3 worldPos)
    {
        Vector3 player = PlayerVrCockpit.Instance.transform.position;
        Vector3 diff = worldPos - player;
        diff = Quaternion.Inverse(PlayerVrCockpit.Instance.transform.rotation) * diff;
        diff = diff * CANVAS_WIDTH_HEIGHT / _scanRange;
        //UI��ʊO�̂��͕̂\�����Ȃ�
        if (diff.x > CANVAS_WIDTH_HEIGHT || diff.z > CANVAS_WIDTH_HEIGHT) return;
            

        VrRadarIconUI icon = _icons.Where(a => !a.gameObject.activeSelf).FirstOrDefault();
        if (icon == null)
        {
            icon = Instantiate(_iconPrefab, this.transform);
            _icons.Add(icon);
        }
        icon.transform.localPosition = new Vector3(diff.x, diff.z, 0);
        icon.Show(_scanSpeed);
    }
}