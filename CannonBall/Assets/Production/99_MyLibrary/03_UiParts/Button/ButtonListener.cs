/*******************************************************************************
*
*	�^�C�g���F	�{�^�����X�i�[�X�N���v�g
*	�t�@�C���F	ButtonListener.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonListener : UiListenerBase
{
    /// <summary> �{�^�������肵���Ƃ��̃R�[���o�b�N </summary>
    [SerializeField]
    private UnityEvent m_onClickCallBack;

    public UnityEvent m_OnClickCallBack { get { return m_onClickCallBack; } }


    protected void Awake()
    {
        InitUiPattern(UiPattern.Button);
    }

    /// <summary>
    /// ���菈��
    /// </summary>
    public override void Submit()
    {
        m_onClickCallBack.Invoke();
    }
}
