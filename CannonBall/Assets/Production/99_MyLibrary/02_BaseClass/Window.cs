/*******************************************************************************
*
*	�^�C�g���F	�E�B���h�E����X�N���v�g
*	�t�@�C���F	Window.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/04/16
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Window : InputElement
{
    /// <summary> �E�B���h�E�\���E��\�����p�̃A�j���[�V���� </summary>
    private Animator m_animator;

    /// <summary> 1�O�̃E�B���h�E(�O�̃E�B���h�E�֖߂�ۂɎg�p) </summary>
    private Window m_beforeWindow;

    /// <summary> ��ʑJ�ڂ��\�ȏ�Ԃ� </summary>
    private bool m_isCanTransition = true;

    /// <summary> �C���v�b�g�A�N�V�����}�b�v </summary>
    protected NewInputActionMap m_inputActionMap;

    public bool m_IsCanTransition
    {
        get { return m_isCanTransition; }
    }

    public NewInputActionMap m_InputActionMap
    {
        get { return m_inputActionMap; }
        set { m_inputActionMap = value; }
    }


    protected virtual void Awake()
    {
        TryGetComponent(out m_animator);
    }

    /// <summary>
    /// �C���v�b�g�A�N�V�����}�b�v�̏�����
    /// </summary>
    public void InitInputActionMap(NewInputActionMap map)
    {
        m_inputActionMap = map;
    }

    /// <summary>
    /// �E�B���h�E���J��
    /// </summary>
    /// <param name="window"> �J�ڌ��E�B���h�E(�J�ڌ��E�B���h�E���������ƂőJ�ڂ����ꍇ�͕K�v�Ȃ�) </param>
    /// <param name="isUnlockGroup"> �O���[�v�̓��̓��b�N���������邩 </param>
    protected virtual void Open(Window window = null, bool isUnlockGroup = true)
    {
        // �J�ڌ��E�B���h�E���n����Ă���ꍇ�̂ݐݒ�
        if (window != null)
        {
            m_beforeWindow = window;
        }

        // �A�j���[�^�[�̐ݒ�
        if (m_animator != null)
        {
            m_animator.SetBool("IsOpen", true);
            m_isCanTransition = true;
        }
        gameObject.SetActive(true);

        // �O���[�v�̓��̓��b�N�̉���
        if (isUnlockGroup)
        {
            m_Manager.UnlockInputGroupOrder(m_Group, false);
        }
    }

    /// <summary>
    /// �E�B���h�E�����
    /// </summary>
    /// <param name="isLockGroup"> �O���[�v�̓��̓��b�N���������邩 </param>
    protected virtual void Close(bool isLockGroup = true)
    {
        // �A�j���[�^�[�̐ݒ�
        if (m_animator != null)
        {
            m_animator.SetBool("IsOpen", false);
            m_isCanTransition = false;
        }
        else
        {
            gameObject.SetActive(false);
        }

        // �O���[�v�̓��͂����b�N
        if (isLockGroup)
        {
            m_Manager.LockInputGroupOrder(false);
        }
    }

    /// <summary>
    /// �w�肵���E�B���h�E�ɑJ��
    /// </summary>
    /// <param name="window"> �J�ڐ�E�B���h�E </param>
    /// <returns> �J�ڂ����s���ꂽ�� </returns>
    protected virtual bool NextWindowChange(Window window)
    {
        bool isTransition = m_isCanTransition && window.m_IsCanTransition;

        if (isTransition)
        {
            Close();
            window.Open(this);
        }

        return isTransition;
    }

    /// <summary>
    /// �w�肵���|�b�v�A�b�v�E�B���h�E��\��
    /// </summary>
    /// <param name="window"> �\���E�B���h�E </param>
    /// <returns> �J�ڂ����s���ꂽ�� </returns>
    protected virtual bool PopupWindowOpen(Window window)
    {
        bool isTransition = m_isCanTransition && window.m_IsCanTransition;

        if (m_isCanTransition && window.m_IsCanTransition)
        {
            m_Manager.LockInputGroupOrder(false);
            window.Open(this);

            // �T�E���h�Đ�
            AudioManager.Instance.PlaySe("�A�E�g�Q�[��_�|�b�v�A�b�v�E�B���h�E�\����", false);
        }

        return isTransition;
    }

    /// <summary>
    /// 1�O�̃E�B���h�E�ɖ߂�
    /// </summary>
    /// <returns> �J�ڂ����s���ꂽ�� </returns>
    protected virtual bool BeforeWindowChange()
    {
        bool isTransition = m_isCanTransition;

        if (isTransition)
        {
            Close();
            m_beforeWindow.Open();
        }

        return isTransition;
    }

    /// <summary>
    /// ���鏈���̏I�����R�[���o�b�N�֐�
    /// </summary>
    public virtual void OpenComplete()
    {
        m_isCanTransition = true;
    }

    /// <summary>
    /// ���鏈���̏I�����R�[���o�b�N�֐�
    /// </summary>
    public virtual void CloseComplete()
    {
        m_isCanTransition = true;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �A�N�V�����}�b�v��L����
    /// </summary>
    public virtual void EnableInputActionMap()
    {
        m_inputActionMap.Enable();
    }

    /// <summary>
    /// �A�N�V�����}�b�v�𖳌���
    /// </summary>
    public virtual void DisableInputActionMap()
    {
        m_inputActionMap.Disable();
    }
}
