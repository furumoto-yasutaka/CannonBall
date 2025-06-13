/*******************************************************************************
*
*	�^�C�g���F	Ui���X�i�[���N���X
*	�t�@�C���F	UiListenerBase.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiListenerBase : MonoBehaviour
{
    public enum UiPattern
    {
        Select = 0,
        Button,
        RadioButton,
        Slider,
        SlideSwitch,
    }


    /// <summary> UI�O���[�v��ID </summary>
    private int m_index;

    private UiPattern m_uiPattern;


    public int m_Index { get { return m_index; } }

    public UiPattern m_UiPattern { get { return m_uiPattern; } }

    /// <summary>
    /// UI��̐ݒ�
    /// </summary>
    protected void InitUiPattern(UiPattern pattern)
    {
        m_uiPattern = pattern;
    }

    /// <summary>
    /// ID�̏�����
    /// </summary>
    public void InitIndex(int id)
    {
        m_index = id;
    }

    /// <summary>
    /// ���g��I����Ԃɂ���
    /// </summary>
    public virtual void Select() { }

    /// <summary>
    /// ���g���I����Ԃɂ���
    /// </summary>
    public virtual void Unselect() { }

    /// <summary>
    /// ���菈��
    /// </summary>
    public virtual void Submit() { }

    /// <summary>
    /// �L�����Z������
    /// </summary>
    public virtual void Cancel() { }

    /// <summary>
    /// ���ɃX���C�h
    /// </summary>
    public virtual void LeftSlide() { }

    /// <summary>
    /// ���ɃX���C�h
    /// </summary>
    public virtual void RightSlide() { }

    /// <summary>
    /// �X�C�b�`�̏�Ԃ�ύX
    /// </summary>
    protected virtual void ChangeSwitch() { }
}
