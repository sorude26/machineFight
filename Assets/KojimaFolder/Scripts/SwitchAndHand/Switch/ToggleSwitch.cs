using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オンの時はレバーがy軸上方向、オフの時はy軸下方向に傾くようにプレハブ作らないとバグが発生する
/// </summary>
public class ToggleSwitch : Switch
{
    enum AdvancedGrabType
    {
        Nomal,
        Thumb,
        Index
    }
    [SerializeField]
    Transform _onPosition;
    [SerializeField]
    Transform _offPosition;
    [SerializeField]
    Transform _switchMovablePartObject;
    [SerializeField]
    HandPoseWeights _thumbHandPose;
    [SerializeField]
    HandPoseWeights _IndexHandPose;
    AdvancedGrabType _currentAdvancedGrabType;

    public override HoldTypes HoldType 
    { get
        {
            switch (_currentAdvancedGrabType)
            {
                case AdvancedGrabType.Nomal:
                    return HoldTypes.Pinch;
                case AdvancedGrabType.Thumb:
                    return HoldTypes.Thumb;
                case AdvancedGrabType.Index:
                    return HoldTypes.Pinch;
                default:
                    Debug.LogError("ToggleSwitchでエラー");
                    return HoldTypes.Pinch;
            }
        } 
    }

    public override HandPoseWeights GetPose()
    {
        switch (_currentAdvancedGrabType)
        {
            case AdvancedGrabType.Nomal:
                return base.GetPose();
            case AdvancedGrabType.Thumb:
                return _thumbHandPose;
            case AdvancedGrabType.Index:
                return _IndexHandPose;
            default:
                Debug.LogError("ToggleSwitch.GetPoseでエラー");
                return base.GetPose();
        }
    }

    [ContextMenu("TurnOn")]
    void TurnOnTest()
    {
        TurnOn();
    }
    [ContextMenu("TurnOff")]
    void TurnOffTest()
    {
        TurnOff();
    }

    public override void TurnOn(bool isInit = false)
    {
        base.TurnOn(isInit);
        _switchMovablePartObject.transform.parent = _onPosition;
        _switchMovablePartObject.transform.localPosition = Vector3.zero;
        _switchMovablePartObject.transform.localRotation = Quaternion.identity;
    }

    
    public override void TurnOff(bool isInit = false)
    {
        base.TurnOff(isInit);
        _switchMovablePartObject.transform.parent = _offPosition;
        _switchMovablePartObject.transform.localPosition = Vector3.zero;
        _switchMovablePartObject.transform.localRotation = Quaternion.identity;
    }



    protected override void Awake()
    {
        base.Awake();
        TurnOff(true);
    }

    protected override void SwitchUpdateOnLockIn()
    {
        base.SwitchUpdateOnLockIn();
        Vector3 local = transform.InverseTransformPoint(_lockinHand.ReferencePositionWorld);
        bool _beforeValue = IsOn;
        if (local.y > 0)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }

        if (base.GetHoldInInput(_lockinHand, HoldTypes.Pinch))
        {
            _currentAdvancedGrabType = AdvancedGrabType.Nomal;
        }
        //弾き入力の場合は値が変わった時にFreeする
        if ((IsOn != _beforeValue) && (_currentAdvancedGrabType != AdvancedGrabType.Nomal))
        {
            Free();
        }
    }

    protected override bool GetHoldInInput(SwitchCtrlHand from, HoldTypes hold)
    {
        //一旦Nomalにしておかないと入力がうまくいかない可能性がある
        _currentAdvancedGrabType = AdvancedGrabType.Nomal;
        //通常のつまみ入力があればそのまま返却
        if (base.GetHoldInInput(from, hold))
        {
            _currentAdvancedGrabType = AdvancedGrabType.Nomal;
            return true;
        }

        //以下弾き入力判別処理
        bool thumbInput = OVRInput.GetDown(OVRInput.Button.Two, from.ControllerType);
        bool indexInput = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, from.ControllerType);
        Vector3 rotate = (from.transform.parent.rotation * Quaternion.Inverse(this.transform.rotation))
            .eulerAngles;
        float rotateZ = rotate.NomalizeRotate180().z;
        if (IsOn)
        {
            //On状態なので親指か人差し指が上から押し下げる形になって入力が来ているかチェック
            //親指(-90～90)
            if (rotateZ < 90f && rotateZ > -90 && thumbInput)
            {
                _currentAdvancedGrabType = AdvancedGrabType.Thumb;
                return true;
            }

            //人差し指
            //右手と左手で角度が違うので厄介 左手=>-180～0の間で判定、 右手=>0～180の間で判定
            if (from.IsRightHand)
            {
                //右手(0～180) 角度は-180～180で正規化されているので正の値か判別するだけで良い
                if (rotateZ > 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
            else
            {
                //左手(0～-180) 角度は-180～180で正規化されているので負の値か判別するだけで良い
                if (rotateZ < 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
        }
        else
        {
            //Off状態なので親指か人差し指が下から押し上げる形になっているかチェック

            //親指(90～270)
            float zeroTo360 = rotateZ < 0f ? 360f + rotateZ : rotateZ;
            if (zeroTo360 > 90f && zeroTo360 < 270f && thumbInput)
            {
                _currentAdvancedGrabType = AdvancedGrabType.Thumb;
                return true;
            }

            //人差し指
            if (from.IsRightHand)
            {
                //右手(-180～0)
                if (rotateZ < 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
            else
            {
                //左手(0～180)
                if (rotateZ > 0 && indexInput)
                {
                    _currentAdvancedGrabType = AdvancedGrabType.Index;
                    return true;
                }
            }
        }
        return false;
    }

    protected override bool GetFreeInput(SwitchCtrlHand from, HoldTypes hold)
    {
        switch (_currentAdvancedGrabType)
        {
            case AdvancedGrabType.Nomal:
                return base.GetFreeInput(from, hold);
            case AdvancedGrabType.Thumb:
                return OculusGameInput.GetThumbOut(from.ControllerType);
            case AdvancedGrabType.Index:
                return !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, from.ControllerType);
            default:
                Debug.LogError("ToggleSwitchでエラー");
                return true;
        }
    }
}
