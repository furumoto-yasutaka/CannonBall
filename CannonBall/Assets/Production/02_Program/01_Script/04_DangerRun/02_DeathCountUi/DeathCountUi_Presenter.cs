/*******************************************************************************
*
*	�^�C�g���F	���S�񐔕\�����䒇��X�N���v�g
*	�t�@�C���F	DeathCountUi_Presenter.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/10
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DeathCountUi_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> ���S���\�� 1P </summary>
    [SerializeField, CustomLabel("���S���\�� 1P")]
    private DeachCountUi_View m_view_1P;

    /// <summary> ���S���\�� 2P </summary>
    [SerializeField, CustomLabel("���S���\�� 2P")]
    private DeachCountUi_View m_view_2P;

    /// <summary> ���S���\�� 3P </summary>
    [SerializeField, CustomLabel("���S���\�� 3P")]
    private DeachCountUi_View m_view_3P;

    /// <summary> ���S���\�� 4P </summary>
    [SerializeField, CustomLabel("���S���\�� 4P")]
    private DeachCountUi_View m_view_4P;


    [Header("Model")]

    /// <summary> ���S������ 1P </summary>
    [SerializeField, CustomLabel("���S������ 1P")]
    private PlayerDeathCount m_deathCount_1P;

    /// <summary> ���S������ 2P </summary>
    [SerializeField, CustomLabel("���S������ 2P")]
    private PlayerDeathCount m_deathCount_2P;

    /// <summary> ���S������ 3P </summary>
    [SerializeField, CustomLabel("���S������ 3P")]
    private PlayerDeathCount m_deathCount_3P;

    /// <summary> ���S������ 4P </summary>
    [SerializeField, CustomLabel("���S������ 4P")]
    private PlayerDeathCount m_deathCount_4P;


    private void Start()
    {
        m_view_1P.Init();
        m_view_2P.Init();
        m_view_3P.Init();
        m_view_4P.Init();

        m_deathCount_1P.m_DeathCount.Subscribe(v =>
            {
                m_view_1P.SetValue(v);
            })
            .AddTo(this);
        m_deathCount_2P.m_DeathCount.Subscribe(v =>
            {
                m_view_2P.SetValue(v);
            })
            .AddTo(this);
        m_deathCount_3P.m_DeathCount.Subscribe(v =>
            {
                m_view_3P.SetValue(v);
            })
            .AddTo(this);
        m_deathCount_4P.m_DeathCount.Subscribe(v =>
            {
                m_view_4P.SetValue(v);
            })
            .AddTo(this);
    }
}
