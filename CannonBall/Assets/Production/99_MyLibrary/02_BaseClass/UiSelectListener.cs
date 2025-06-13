/*******************************************************************************
*
*	�^�C�g���F	Ui���X�i�[���N���X
*	�t�@�C���F	UiSelectListener.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2024/01/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSelectListener : UiListenerBase
{
    /// <summary> �I�����p�A�j���[�^�[ </summary>
    protected Animator m_animator;


    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
        InitUiPattern(UiPattern.Select);
    }

    /// <summary>
    /// ���g��I����Ԃɂ���
    /// </summary>
    public override void Select()
    {
        m_animator.SetBool("IsSelect", true);
    }

    /// <summary>
    /// ���g���I����Ԃɂ���
    /// </summary>
    public override void Unselect()
    {
        m_animator.SetBool("IsSelect", false);
    }
}
