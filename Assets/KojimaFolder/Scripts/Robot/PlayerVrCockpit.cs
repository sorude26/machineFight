using Oculus.Interaction.Input;
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

        /// <summary>
        /// 右手
        /// </summary>
        /// <returns></returns>
        public static bool Attack1()
        {
            return _flightStick.GetTriggerInput(false);
        }
        /// <summary>
        /// 左手
        /// </summary>
        /// <returns></returns>
        public static bool Attack2()
        {
            return _flightStick.GetTriggerInput(false);
        }
        /// <summary>
        /// バックウェポンorホバー
        /// </summary>
        /// <returns></returns>
        public static bool Attack3()
        {
            return false;
        }
        /// <summary>
        /// キック
        /// </summary>
        /// <returns></returns>
        public static bool Attack4()
        {
            return false;
        }
        /// <summary>
        /// ジャンプ
        /// </summary>
        /// <returns></returns>
        public static bool Jump()
        {
            return false;
        }
        /// <summary>
        /// ステップ
        /// </summary>
        /// <returns></returns>
        public static bool JetBoost()
        {
            return false;
        }
        /// <summary>
        /// ターゲット切り替え
        /// </summary>
        /// <returns></returns>
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
        CameraSetup();
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

    private void CameraSetup()
    {
        //VR機器が接続されていない場合はデスクトップ用のカメラに切り替え、コックピットを非表示にする。
        if (!OVRManager.isHmdPresent)
        {
            var cameras = FindObjectsOfType<Camera>(true);
            foreach (var item in cameras)
            {
                item.targetTexture = null;
            }
            this.gameObject.SetActive(false);
        }
    }
}
