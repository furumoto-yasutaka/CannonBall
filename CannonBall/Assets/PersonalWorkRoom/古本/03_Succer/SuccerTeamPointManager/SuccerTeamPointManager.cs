/*******************************************************************************
*
*	�^�C�g���F	�T�b�J�[�̓��_�Ǘ��V���O���g���X�N���v�g
*	�t�@�C���F	PointManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SuccerTeamPointManager : SingletonMonoBehaviour<SuccerTeamPointManager>
{
    /// <summary> �Ԃ̓��_ </summary>
    [SerializeField, CustomLabelReadOnly("�Ԃ̓��_")]
    private ReactiveProperty<int> m_redPoint = new ReactiveProperty<int>(0);

    /// <summary> �Ԃ̓��_ </summary>
    [SerializeField, CustomLabelReadOnly("�̓��_")]
    private ReactiveProperty<int> m_bluePoint = new ReactiveProperty<int>(0);

    /// <summary> �{�[�����������Ƃ��̃|�C���g </summary>
    [SerializeField, CustomLabel("�{�[�����������Ƃ��̃|�C���g")]
    private int m_ballInPoint = 4;

    /// <summary> �{�[�����������Ƃ��̃|�C���g </summary>
    [SerializeField, CustomLabel("���{�[�����������Ƃ��̃|�C���g")]
    private int m_rareballInPoint = 4;

    /// <summary> �v���C���[���������Ƃ��̃|�C���g </summary>
    [SerializeField, CustomLabel("�v���C���[���������Ƃ��̃|�C���g")]
    private int m_playerInPoint = 1;

    /// <summary> �����I���̃{�[�_�[�̃|�C���g </summary>
    [SerializeField, CustomLabel("�����I���̃{�[�_�[�̃|�C���g")]
    private int m_finishBorderPoint = 30;

    /// <summary> �������I���������ǂ��� </summary>
    private bool m_isFinish = false;


    public IReadOnlyReactiveProperty<int> m_RedPoint => m_redPoint;

    public IReadOnlyReactiveProperty<int> m_BluePoint => m_bluePoint;

    public bool m_IsFinish => m_isFinish;


    /// <summary> �ԃ`�[���֒ʏ�{�[���̃S�[���|�C���g�����Z </summary>
    public void BallGoalIn_Red()
    {
        if (m_isFinish) { return; }
        m_redPoint.Value += m_ballInPoint;
        CheckFinish(m_redPoint.Value);
    }

    /// <summary> �`�[���֒ʏ�{�[���̃S�[���|�C���g�����Z </summary>
    public void BallGoalIn_Blue()
    {
        if (m_isFinish) { return; }
        m_bluePoint.Value += m_ballInPoint;
        CheckFinish(m_bluePoint.Value);
    }

    /// <summary> �ԃ`�[���֋��{�[���̃S�[���|�C���g�����Z </summary>
    public void RareBallGoalIn_Red()
    {
        if (m_isFinish) { return; }
        m_redPoint.Value += m_rareballInPoint;
        CheckFinish(m_redPoint.Value);
    }

    /// <summary> �`�[���֋��{�[���̃S�[���|�C���g�����Z </summary>
    public void RareBallGoalIn_Blue()
    {
        if (m_isFinish) { return; }
        m_bluePoint.Value += m_rareballInPoint;
        CheckFinish(m_bluePoint.Value);
    }

    /// <summary> �ԃ`�[���֑���v���C���[���ł̃|�C���g�����Z </summary>
    public void PlayerGoalIn_Red()
    {
        if (m_isFinish) { return; }
        m_redPoint.Value += m_playerInPoint;
        CheckFinish(m_redPoint.Value);
    }

    /// <summary> �`�[���֑���v���C���[���ł̃|�C���g�����Z </summary>
    public void PlayerGoalIn_Blue()
    {
        if (m_isFinish) { return; }
        m_bluePoint.Value += m_playerInPoint;
        CheckFinish(m_bluePoint.Value);
    }

    private void CheckFinish(int point)
    {
        if (point >= m_finishBorderPoint)
        {
            m_isFinish = true;
        }
    }
}
