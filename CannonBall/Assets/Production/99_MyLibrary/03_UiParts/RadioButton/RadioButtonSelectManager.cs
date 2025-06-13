/*******************************************************************************
*
*	�^�C�g���F	���W�I�{�^���O���[�v����X�N���v�g(�ėp)
*	�t�@�C���F	RadioButtonSelectManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioButtonSelectManager : UiSelectManager
{
    /// <summary> �����Ԃɂ���UI </summary>
    protected int m_submitIndex = 0;

    /// <summary> �C���v�b�g�A�N�V����(�������) </summary>
    private NewInputAction_Button m_inputAct_Submit;


    protected override void Start()
    {
        base.Start();

        InitSubmitIndex();

        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_Ui.Button.Submit.ToString());
    }

    /// <summary>
    /// �����Ԃ̏�����
    /// </summary>
    private void InitSubmitIndex()
    {
        // ���Z�[�u�f�[�^����擾
        //m_submitIndex = SaveDataManager.Instance.m_SaveData.Value.Option.KeyConfigPattern;
        ((RadioButtonListener)m_listener[m_submitIndex]).Submit();
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
            m_listener[i] = trans.GetComponent<RadioButtonListener>();
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
    public virtual void Submit()
    {
        ((RadioButtonListener)m_listener[m_submitIndex]).Cancel();

        m_submitIndex = m_selectCursorIndex;

        ((RadioButtonListener)m_listener[m_selectCursorIndex]).Submit();

        // �T�E���h�Đ�
        AudioManager.Instance.PlaySe("�A�E�g�Q�[��_���艹", false);
    }
}
