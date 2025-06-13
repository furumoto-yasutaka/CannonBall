/*******************************************************************************
*
*	�^�C�g���F	UI�O���[�v����X�N���v�g
*	�t�@�C���F	UiSelectManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiSelectManager : InputElement
{
    #region enum

    /// <summary> UI�̕��� </summary>
    public enum UiOrder
    {
        Vertical = 0,
        Horizontal,
        Length,
    }

    /// <summary> �A�����͗p�X�e�[�g </summary>
    public enum InputState
    {
        None = 0,
        Wait,
        Interval,
    }

    /// <summary> ���͏�� </summary>
    public enum InputPattern
    {
        None = 0,
        Plus = 1 << 0,
        Minus = 1 << 1,
        Submit = 1 << 2,
    }

    #endregion

    #region field

    /// <summary> UI�̕��ѕ��� </summary>
    [SerializeField, CustomLabel("UI�̕��ѕ���")]
    protected UiOrder m_buttonOrder = UiOrder.Vertical;

    /// <summary> �J�[�\�������[�v�\�ɂ��邩 </summary>
    [SerializeField, CustomLabel("�J�[�\�������[�v�\�ɂ��邩")]
    protected bool m_isLoop = false;

    /// <summary> UI���X�i�[�X�N���v�g </summary>
    protected UiListenerBase[] m_listener;
    /// <summary> �I����Ԃ̃{�^��Id </summary>
    public int m_selectCursorIndex = 0;

    /// <summary> ���͊m�F�֐�(�{�^���̕��т��Ƃɗp��) </summary>
    protected System.Action[] m_checkInput;
    /// <summary> ����������͂̋����̂������l </summary>
    protected const float m_inputMoveThreshold = 0.7f;
    /// <summary> �A�����͊J�n�҂����� </summary>
    protected const float m_continueWaitTime = 0.5f;
    /// <summary> �A�����͑҂����� </summary>
    protected const float m_continueIntervalTime = 0.2f;

    /// <summary> �C���v�b�g�A�N�V����(�J�[�\��) </summary>
    protected NewInputAction_Vector2 m_inputAct_Cursor;
    /// <summary> �A�����͗p�X�e�[�g </summary>
    protected InputState m_inputState = InputState.None;
    /// <summary> �ǂ̃{�^����������Ă��邩�̃r�b�g�p�^�[�� </summary>
    protected int m_inputPattern = (int)InputPattern.None;
    /// <summary> �c�莞�� </summary>
    protected float m_continueTimeCount = 0.0f;
    /// <summary> �N���㏉�߂ẴA�N�e�B�u��Ԃ� </summary>
    protected bool m_isFirstEnable = true;

    #endregion

    #region function

    protected virtual void Start()
    {
        InitListener();
        FirstSelect();

        m_inputAct_Cursor = GetActionMap().GetAction_Vec2(NewInputActionName_Ui.Vector2.Cursor.ToString());

        // �f���Q�[�g�Ɋ֐��ݒ�
        m_checkInput = new System.Action[(int)UiOrder.Length]
            { CheckInput_Vertical, CheckInput_Horizontal };
    }

    protected virtual void OnEnable()
    {
        // �ŏ��̃A�N�e�B�u���̍ۂ͏��������Ԃɍ��킸�A
        // �Q�ƃG���[�ɂȂ�̂ł��Ȃ�
        if (m_isFirstEnable)
        {
            m_isFirstEnable = false;
        }
        else
        {
            // �ŏ��̗v�f��I������
            FirstSelect();
        }
    }

    protected virtual void Update()
    {
        if (!m_IsCanInput) { return; }

        // ���͎擾
        CheckInput();
        // ���͂ɉ��������������s
        Execute();
    }

    /// <summary>
    /// �q�I�u�W�F�N�g���烊�X�i�[���擾���A������
    /// </summary>
    protected virtual void InitListener()
    {
        m_listener = new UiListenerBase[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            m_listener[i] = trans.GetComponent<UiListenerBase>();
            // ID��t�^
            m_listener[i].InitIndex(i);
        }
    }

    /// <summary>
    /// ��ԍŏ��̗v�f��I��
    /// </summary>
    protected void FirstSelect()
    {
        if (transform.childCount == 0)
        {
            // �{�^�������݂��Ȃ��ꍇ�͓��͂��󂯕t���Ȃ��悤�ɂ���
            LockInput();
        }
        else
        {
            // �{�^�������݂���ꍇ�͈�ԍŏ��̃{�^����I����Ԃɂ���
            Select(m_selectCursorIndex);
        }
    }

    /// <summary>
    /// ���͊m�F
    /// </summary>
    protected virtual void CheckInput()
    {
        m_checkInput[(int)m_buttonOrder]();
    }

    /// <summary>
    /// ���͊m�F(�c����)
    /// </summary>
    protected void CheckInput_Vertical()
    {
        float vertical = m_inputAct_Cursor.GetVec2All().y;

        m_inputPattern = (int)InputPattern.None;

        if (vertical >= m_inputMoveThreshold)
        {
            m_inputPattern |= (int)InputPattern.Minus;
        }
        else if (vertical <= -m_inputMoveThreshold)
        {
            m_inputPattern |= (int)InputPattern.Plus;
        }
    }

    /// <summary>
    /// ���͊m�F(������)
    /// </summary>
    protected void CheckInput_Horizontal()
    {
        float horizontal = m_inputAct_Cursor.GetVec2All().x;

        m_inputPattern = (int)InputPattern.None;

        if (horizontal <= -m_inputMoveThreshold)
        {
            m_inputPattern |= (int)InputPattern.Minus;
        }
        else if (horizontal >= m_inputMoveThreshold)
        {
            m_inputPattern |= (int)InputPattern.Plus;
        }
    }

    /// <summary>
    /// ���͂ɉ������������s
    /// </summary>
    protected virtual void Execute()
    {
        if ((m_inputPattern & (int)InputPattern.Plus) > 0 ||
            (m_inputPattern & (int)InputPattern.Minus) > 0)
        {
            // �J�[�\���ړ�
            switch (m_inputState)
            {
                case InputState.None:
                    // �A�����͑҂��ɂ���
                    m_inputState = InputState.Wait;
                    m_continueTimeCount = m_continueWaitTime;

                    // �J�[�\���ړ�
                    MoveCursor();
                    break;
                case InputState.Wait:
                    if (m_continueTimeCount <= 0.0f)
                    {
                        // �A�����͂��J�n����
                        m_inputState = InputState.Interval;
                        m_continueTimeCount = m_continueIntervalTime;

                        // �J�[�\���ړ�
                        MoveCursor();
                    }
                    else
                    {
                        m_continueTimeCount -= Time.deltaTime;
                    }
                    break;
                case InputState.Interval:
                    if (m_continueTimeCount <= 0.0f)
                    {
                        m_continueTimeCount = m_continueIntervalTime;

                        // �J�[�\���ړ�
                        MoveCursor();
                    }
                    else
                    {
                        m_continueTimeCount -= Time.deltaTime;
                    }
                    break;
            }
        }
        else
        {
            // �J�[�\���ړ��̃{�^����������Ă��Ȃ��ꍇ�A�����͌n�p�����[�^��������
            m_continueTimeCount = 0.0f;
            m_inputState = InputState.None;
        }
    }

    /// <summary>
    /// �J�[�\���ړ�
    /// </summary>
    protected void MoveCursor()
    {
        if ((m_inputPattern & (int)InputPattern.Plus) > 0)
        {
            // ���[�v�s�ݒ�Ń��[�v���N����V�`���G�[�V�����̏ꍇ�͏��������Ȃ�
            if (!m_isLoop && m_selectCursorIndex == transform.childCount - 1) { return; }

            Select((m_selectCursorIndex + 1) % transform.childCount);

            //// �T�E���h�Đ�
            //AudioManager.Instance.PlaySe("�A�E�g�Q�[��_�J�[�\���ړ���", false);
        }
        else if ((m_inputPattern & (int)InputPattern.Minus) > 0)
        {
            // ���[�v�s�ݒ�Ń��[�v���N����V�`���G�[�V�����̏ꍇ�͏��������Ȃ�
            if (!m_isLoop && m_selectCursorIndex == 0) { return; }

            Select((m_selectCursorIndex - 1 + transform.childCount) % transform.childCount);

            //// �T�E���h�Đ�
            //AudioManager.Instance.PlaySe("�A�E�g�Q�[��_�J�[�\���ړ���", false);
        }
    }

    /// <summary>
    /// �I������
    /// </summary>
    public virtual void Select(int index)
    {
        // ���݂̑I��������
        m_listener[m_selectCursorIndex].Unselect();
        m_selectCursorIndex = index;
        m_listener[m_selectCursorIndex].Select();
    }

    #endregion
}
