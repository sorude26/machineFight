using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVrCockpitInMenu : PlayerVrCockpit
{
    const float BORDER = 0.85f;
    /// <summary>
    /// ���j���[�ł͉E�����ւ̃Z���N�g�Ɏg�p����邽�߃J����Axis���g�p
    /// </summary>
    /// <returns></returns>
    protected override bool Attack1Virtual()
    {
        //return _flightStick.GetThumbstickInput().x > BORDER;
        return false;
    }
    /// <summary>
    /// ���j���[�ł͍������ւ̃Z���N�g�Ɏg�p����邽�߃J����Axis���g�p
    /// </summary>
    /// <returns></returns>
    protected override bool Attack2Virtual()
    {
        //return _flightStick.GetThumbstickInput().x < -BORDER;
        return false;
    }

    /// <summary>
    /// ���j���[�̑I���Ɏg�p���邽�߃t���C�g�X�e�B�b�N�㕔�̃X�e�B�b�N���g�p
    /// </summary>
    /// <returns></returns>
    protected override Vector2 MoveVirtual()
    {
        return _flightStick.GetThumbstickInput();
    }
}
