/*******************************************************************************
*
*	�^�C�g���F	�{�^���O���[�v����X�N���v�g(�ėp)
*	�t�@�C���F	ButtonSelectManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonSelectManager : UiSelectManager
{
    /// <summary> �C���v�b�g�A�N�V����(�������) </summary>
    private NewInputAction_Button m_inputAct_Submit;


    protected override void Start()
    {
        base.Start();

        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_Ui.Button.Submit.ToString());
    }

    /// <summary>
    /// �q�I�u�W�F�N�g���烊�X�i�[���擾���A������
    /// </summary>
    protected override void InitListener()
    {
        m_listener = new UiListenerBase[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            m_listener[i] = trans.GetComponent<ButtonListener>();
            m_listener[i].InitIndex(i);
        }
    }

    /// <summary>
    /// ���͊m�F
    /// </summary>
    protected override void CheckInput()
    {
        base.CheckInput();
        CheckInput_Submit();
    }

    /// <summary>
    /// ���͊m�F(�������)
    /// </summary>
    private void CheckInput_Submit()
    {
        if (m_inputAct_Submit.GetDownAll())
        {
            m_inputPattern |= (int)InputPattern.Submit;
        }
    }

    /// <summary>
    /// ���͂ɉ������������s
    /// </summary>
    protected override void Execute()
    {
        base.Execute();

        if ((m_inputPattern & (int)InputPattern.Submit) > 0)
        {
            // ���菈��
            Submit();
        }
    }

    /// <summary>
    /// ���菈��
    /// </summary>
    public void Submit()
    {
        ((ButtonListener)m_listener[m_selectCursorIndex]).Submit();
    }
}
