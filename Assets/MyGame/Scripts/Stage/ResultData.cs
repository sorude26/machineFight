using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���U���g�f�[�^�N���X
/// </summary>
public static class ResultData
{
    /// <summary>
    /// �X�e�[�W�ł̃g�[�^����_���[�W
    /// </summary>
    public static int TotalDamage { get; set; }
    /// <summary>
    /// �����j��
    /// </summary>
    public static int TotalCount { get; set; }
    /// <summary>
    /// �^�[�Q�b�g�̌��j��
    /// </summary>
    public static int TotalTargetCount { get; set; }
    /// <summary>
    /// �^�[�Q�b�g�̐�
    /// </summary>
    public static int TotalTargetNum { get; set; }
    /// <summary>
    /// �{�X�̌��j��
    /// </summary>
    public static int TotalBossCount { get; set; }
    /// <summary>
    /// �{�X�̐�
    /// </summary>
    public static int TotalBossNum { get; set; }
    /// <summary>
    /// �X�e�[�W���N���A�������̃t���O
    /// </summary>
    public static bool IsStageClear { get; set; }
}
