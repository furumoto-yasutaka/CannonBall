using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerSpMovePoint_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 1P")]
    private PlayerSpMovePoint_View m_view_1P;

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 2P")]
    private PlayerSpMovePoint_View m_view_2P;

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 3P")]
    private PlayerSpMovePoint_View m_view_3P;

    /// <summary> �|�C���g�\������R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�|�C���g�\�� 4P")]
    private PlayerSpMovePoint_View m_view_4P;


    [Header("Model")]

    /// <summary> �v���C���[�̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�v���C���[�̐e�I�u�W�F�N�g")]
    private Transform m_playerParent;

    /// <summary> �K�E�Z�R���|�[�l���g1P </summary>
    private PlayerSpMove m_point_1P;
    /// <summary> �K�E�Z�R���|�[�l���g2P </summary>
    private PlayerSpMove m_point_2P;
    /// <summary> �K�E�Z�R���|�[�l���g3P </summary>
    private PlayerSpMove m_point_3P;
    /// <summary> �K�E�Z�R���|�[�l���g4P </summary>
    private PlayerSpMove m_point_4P;


    private void Awake()
    {
        m_point_1P = m_playerParent.GetChild(0).GetComponent<PlayerSpMove>();
        m_point_1P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_1P.SetSliderValue(v);
            })
            .AddTo(this);
        m_point_2P = m_playerParent.GetChild(1).GetComponent<PlayerSpMove>();
        m_point_2P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_2P.SetSliderValue(v);
            })
            .AddTo(this);
        m_point_3P = m_playerParent.GetChild(2).GetComponent<PlayerSpMove>();
        m_point_3P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_3P.SetSliderValue(v);
            })
            .AddTo(this);
        m_point_4P = m_playerParent.GetChild(3).GetComponent<PlayerSpMove>();
        m_point_4P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_4P.SetSliderValue(v);
            })
            .AddTo(this);
    }
}
