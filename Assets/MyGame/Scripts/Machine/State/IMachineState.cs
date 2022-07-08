using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        /// <summary>
        /// Machine�̃X�e�[�g
        /// </summary>
        protected interface IMachineState
        {
            /// <summary>
            /// �X�e�[�g�J�n��
            /// </summary>
            /// <param name="control"></param>
            void OnEnter(StateController control);
            /// <summary>
            /// �X�e�[�gUpdate
            /// </summary>
            /// <param name="control"></param>
            void OnUpdate(StateController control);
            /// <summary>
            /// �X�e�[�g�ړ��֌WUpdate
            /// </summary>
            /// <param name="control"></param>
            void OnFixedUpdate(StateController control);
        }
    }
}