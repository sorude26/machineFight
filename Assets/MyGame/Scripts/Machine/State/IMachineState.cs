using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.MachineFrame
{
    public partial class StateController
    {
        /// <summary>
        /// Machineのステート
        /// </summary>
        protected interface IMachineState
        {
            /// <summary>
            /// ステート開始時
            /// </summary>
            /// <param name="control"></param>
            void OnEnter(StateController control);
            /// <summary>
            /// ステートUpdate
            /// </summary>
            /// <param name="control"></param>
            void OnUpdate(StateController control);
            /// <summary>
            /// ステート移動関係Update
            /// </summary>
            /// <param name="control"></param>
            void OnFixedUpdate(StateController control);
        }
    }
}