/*******************************************************************************
*
*	�^�C�g���F	���͎�t�v�f�̊��N���X
*	�t�@�C���F	InputElement.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/04/16
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputElement : MonoBehaviour
{
    /// <summary> �}�l�[�W���[ </summary>
    private InputGroupManager m_manager;

    /// <summary> �����O���[�v </summary>
    private InputGroup m_group;

    /// <summary> ���͉\�� </summary>
    private bool m_isCanInput = true;


    public NewInputActionMap GetActionMap()
    {
        return m_Group.m_Window.m_InputActionMap;
    }


    public InputGroupManager m_Manager
    {
        get { return m_manager; }
    }

    public InputGroup m_Group
    {
        get { return m_group; }
    }

    public bool m_IsCanInput
    {
        get { return m_group.m_IsCanInput && m_isCanInput; }
    }


    /// <summary>
    /// �p�����[�^������
    /// </summary>
    public virtual void InitializeParam(InputGroupManager manager, InputGroup group)
    {
        m_manager = manager;
        m_group = group;
    }

    /// <summary>
    /// �v�f�P�ʂœ��͂����b�N
    /// </summary>
    public virtual void LockInput()
    {
        m_isCanInput = false;
    }

    /// <summary>
    /// �v�f�P�ʂœ��͂̃��b�N����
    /// </summary>
    public virtual void UnlockInput()
    {
        m_isCanInput = true;
    }
}
