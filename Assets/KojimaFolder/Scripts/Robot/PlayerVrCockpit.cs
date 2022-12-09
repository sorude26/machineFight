using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerVrCockpit : MonoBehaviour
{
    private static PlayerVrCockpit _instance;
    public static PlayerVrCockpit Instance => _instance;

    [SerializeField]
    FlightStick _flightStick;

    public static class Input
    {
        static FlightStick _flightStick;
        public static void SetDevice(FlightStick stick)
        {
            _flightStick = stick;
        }

        public static bool Attack1()
        {
            return _flightStick.GetTriggerInput(false);
        }
        public static bool Attack2()
        {
            return _flightStick.GetTriggerInput(false);
        }
        public static bool Attack3()
        {
            return _flightStick.GetTriggerInput(false);
        }
        public static bool Attack4()
        {
            return _flightStick.GetTriggerInput(false);
        }
        public static bool Jump()
        {
            return _flightStick.GetLowerButtonInput(false);
        }
        public static bool JetBoost()
        {
            return _flightStick.GetUpperButtonInput(false);
        }
        public static bool ChangeTarget()
        {
            return false;
        }

        public static Vector2 Move()
        {
            return _flightStick.GetStickBodyInput();
        }

        public static Vector2 Camera()
        {
            return _flightStick.GetThumbsStickInput();
        }
    }

    private void Awake()
    {
        _instance = this;
        var layer = this.gameObject.layer;
        SetLayerToChildlen(layer, this.transform);
        Input.SetDevice(_flightStick);
    }

    private void SetLayerToChildlen(int layer, Transform t)
    {
        foreach (Transform item in t)
        {
            item.gameObject.layer = layer;
            //再帰処理ですべての子オブジェクトに適用
            SetLayerToChildlen(layer, item);
        }
    }
}
