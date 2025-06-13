/*******************************************************************************
*
*	�^�C�g���F	���W�I�{�^�����X�i�[�X�N���v�g
*	�t�@�C���F	RadioButtonListener.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButtonListener : UiListenerBase
{
    /// <summary> �I�����Ă��邱�Ƃ�\���摜 </summary>
    [SerializeField, CustomLabel("�I�����Ă��邱�Ƃ�\���摜")]
    private GameObject m_submitImage;


    protected void Awake()
    {
        InitUiPattern(UiPattern.RadioButton);
    }

    /// <summary>
    /// ���菈��
    /// </summary>
    public override void Submit()
    {
        m_submitImage.SetActive(true);
    }

    /// <summary>
    /// �L�����Z������
    /// </summary>
    public override void Cancel()
    {
        m_submitImage.SetActive(false);
    }
}
