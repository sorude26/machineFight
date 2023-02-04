using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVrCockpitInMenu : PlayerVrCockpit
{
    const float BORDER = 0.85f;
    /// <summary>
    /// メニューでは右方向へのセレクトに使用されるためカメラAxisを使用
    /// </summary>
    /// <returns></returns>
    protected override bool Attack1Virtual()
    {
        //return _flightStick.GetThumbstickInput().x > BORDER;
        return false;
    }
    /// <summary>
    /// メニューでは左方向へのセレクトに使用されるためカメラAxisを使用
    /// </summary>
    /// <returns></returns>
    protected override bool Attack2Virtual()
    {
        //return _flightStick.GetThumbstickInput().x < -BORDER;
        return false;
    }

    /// <summary>
    /// メニューの選択に使用するためフライトスティック上部のスティックを使用
    /// </summary>
    /// <returns></returns>
    protected override Vector2 MoveVirtual()
    {
        return _flightStick.GetThumbstickInput();
    }
}
