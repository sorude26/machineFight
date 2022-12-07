using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerVibrator : MonoBehaviour
{
    static Dictionary<OVRInput.Controller, Timer> _timer = new Dictionary<OVRInput.Controller, Timer>();

    private void Update()
    {
        foreach (var item in _timer)
        {
            item.Value.time -= Time.deltaTime;
            if (item.Value.time < 0)
            {
                OVRInput.SetControllerVibration(0f, 0f, item.Key);
            }
        }
    }

    /// <summary>
    /// �R���g���[���[���o�C�u���[�V����������
    /// </summary>
    /// <param name="frequency">�U�����b0�`1�܂ł̒l�D�l���傫�����U���������Ȃ�</param>
    /// <param name="amplitude">�U���b0�`1�܂ł̒l�D�l���傫�����U�����傫���Ȃ�</param>
    /// <param name="time">����</param>
    /// <param name="controller">�ǂ̃R���g���[���[��U�������邩</param>
    public static void Vibrate(float frequency, float amplitude, float time, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        if (!_timer.ContainsKey(controller))
        {
            _timer.Add(controller, new Timer(time));
        }
        else
        {
            _timer[controller].time = Mathf.Max(_timer[controller].time, time);
        }
    }
}

class Timer
{
    public float time;
    public Timer(float time)
    {
        this.time = time;
    }
}
