/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̃|�C���g�\���p�f�[�^
*	�t�@�C���F	PlayerPoint_CannonFight_Data.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerPoint_CannonFight_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 1P")]
    private PlayerPoint_CannonFight_View m_view_1P;

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 2P")]
    private PlayerPoint_CannonFight_View m_view_2P;

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 3P")]
    private PlayerPoint_CannonFight_View m_view_3P;

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 4P")]
    private PlayerPoint_CannonFight_View m_view_4P;


    [Header("Model")]

    /// <summary> �v���C���[�̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�v���C���[�̐e�I�u�W�F�N�g")]
    private Transform m_playerParent;

    /// <summary> �|�C���g����R���|�[�l���g1P </summary>
    private PlayerPoint_CannonFight m_point_1P;
    /// <summary> �|�C���g����R���|�[�l���g2P </summary>
    private PlayerPoint_CannonFight m_point_2P;
    /// <summary> �|�C���g����R���|�[�l���g3P </summary>
    private PlayerPoint_CannonFight m_point_3P;
    /// <summary> �|�C���g����R���|�[�l���g4P </summary>
    private PlayerPoint_CannonFight m_point_4P;


    private void Awake()
    {
        m_view_1P.Init();
        m_view_2P.Init();
        m_view_3P.Init();
        m_view_4P.Init();

        m_point_1P = m_playerParent.GetChild(0).GetComponent<PlayerPoint_CannonFight>();
        m_point_1P.m_Point.Subscribe(v =>
            {
                m_view_1P.SetValue(v, m_point_1P.m_IsAdd);
            })
            .AddTo(this);
        m_point_2P = m_playerParent.GetChild(1).GetComponent<PlayerPoint_CannonFight>();
        m_point_2P.m_Point.Subscribe(v =>
            {
                m_view_2P.SetValue(v, m_point_2P.m_IsAdd);
            })
            .AddTo(this);
        m_point_3P = m_playerParent.GetChild(2).GetComponent<PlayerPoint_CannonFight>();
        m_point_3P.m_Point.Subscribe(v =>
            {
                m_view_3P.SetValue(v, m_point_3P.m_IsAdd);
            })
            .AddTo(this);
        m_point_4P = m_playerParent.GetChild(3).GetComponent<PlayerPoint_CannonFight>();
        m_point_4P.m_Point.Subscribe(v =>
            {
                m_view_4P.SetValue(v, m_point_4P.m_IsAdd);
            })
            .AddTo(this);
    }
}
