/*******************************************************************************
*
*	�^�C�g���F	�T�b�J�[�̓��_�\�����䒇��X�N���v�g
*	�t�@�C���F	SuccerPointUi_Presenter.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SuccerPointUi_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> �|�C���g�\��(�ԃ`�[��) </summary>
    [SerializeField, CustomLabel("�|�C���g�\��(�ԃ`�[��)")]
    private SuccerPointUi_View m_viewRed;

    /// <summary> �|�C���g�\��(�`�[��) </summary>
    [SerializeField, CustomLabel("�|�C���g�\��(�`�[��)")]
    private SuccerPointUi_View m_viewBlue;


    void Start()
    {
        SuccerTeamPointManager.Instance.m_RedPoint.Subscribe(v =>
            {
                m_viewRed.SetValue(v);
            })
            .AddTo(this);

        SuccerTeamPointManager.Instance.m_BluePoint.Subscribe(v =>
            {
                m_viewBlue.SetValue(v);
            })
            .AddTo(this);
    }
}
