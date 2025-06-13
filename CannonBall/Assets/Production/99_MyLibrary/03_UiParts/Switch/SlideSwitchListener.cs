/*******************************************************************************
*
*	�^�C�g���F	�X���C�h���X�C�b�`���X�i�[�X�N���v�g
*	�t�@�C���F	SlideSwitchListener.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSwitchListener : UiListenerBase
{
    /// <summary> �X�C�b�`�������A�j���[�V���� </summary>
    [SerializeField, CustomLabel("�X�C�b�`�������A�j���[�V����")]
    protected Animator m_switchAnimator = null;

    /// <summary> �X�C�b�`�̓I���� </summary>
    [CustomReadOnly]
    public bool m_IsOn = true;


    protected void Awake()
    {
        InitUiPattern(UiPattern.SlideSwitch);
    }

    /// <summary>
    /// ���ɃX�C�b�`��؂�ւ�
    /// </summary>
    public override void LeftSlide()
    {
        if (!m_IsOn)
        {
            ChangeSwitch();
        }
    }

    /// <summary>
    /// �E�ɃX�C�b�`��؂�ւ�
    /// </summary>
    public override void RightSlide()
    {
        if (m_IsOn)
        {
            ChangeSwitch();
        }
    }

    /// <summary>
    /// �X�C�b�`�̏�Ԃ�ύX
    /// </summary>
    protected override void ChangeSwitch()
    {
        m_IsOn = !m_IsOn;
        SetAnimation();
    }

    /// <summary>
    /// �A�j���[�V�����ݒ�
    /// </summary>
    protected void SetAnimation()
    {
        if (m_switchAnimator != null)
        {
            m_switchAnimator.SetBool("IsOn", m_IsOn);
        }
    }
}
