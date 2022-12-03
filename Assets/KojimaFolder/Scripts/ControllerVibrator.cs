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
    /// コントローラーをバイブレーションさせる
    /// </summary>
    /// <param name="frequency">振動数｜0～1までの値．値が大きい程振動が強くなる</param>
    /// <param name="amplitude">振幅｜0～1までの値．値が大きい程振動が大きくなる</param>
    /// <param name="time">時間</param>
    /// <param name="controller">どのコントローラーを振動させるか</param>
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
