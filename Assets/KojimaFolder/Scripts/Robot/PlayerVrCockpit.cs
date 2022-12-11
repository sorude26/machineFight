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
        /// �E��
        /// </summary>
        /// <returns></returns>
        public static bool Attack1()
        {
            return _flightStick.GetTriggerInput(false);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public static bool Attack2()
        {
            return _flightStick.GetTriggerInput(false);
        }
        /// <summary>
        /// �o�b�N�E�F�|��or�z�o�[
        /// </summary>
        /// <returns></returns>
        public static bool Attack3()
        {
            return false;
        }
        /// <summary>
        /// �L�b�N
        /// </summary>
        /// <returns></returns>
        public static bool Attack4()
        {
            return false;
        }
        /// <summary>
        /// �W�����v
        /// </summary>
        /// <returns></returns>
        public static bool Jump()
        {
            return false;
        }
        /// <summary>
        /// �X�e�b�v
        /// </summary>
        /// <returns></returns>
        public static bool JetBoost()
        {
            return false;
        }
        /// <summary>
        /// �^�[�Q�b�g�؂�ւ�
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
            //�ċA�����ł��ׂĂ̎q�I�u�W�F�N�g�ɓK�p
            SetLayerToChildlen(layer, item);
        }
    }

    private void CameraSetup()
    {
        //VR�@�킪�ڑ�����Ă��Ȃ��ꍇ�̓f�X�N�g�b�v�p�̃J�����ɐ؂�ւ��A�R�b�N�s�b�g���\���ɂ���B
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
