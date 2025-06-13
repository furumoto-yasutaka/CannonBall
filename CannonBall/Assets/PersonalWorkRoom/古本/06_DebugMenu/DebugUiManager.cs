/*******************************************************************************
*
*	�^�C�g���F	�f�o�b�O���j���[UI�O���[�v����X�N���v�g
*	�t�@�C���F	DebugUiManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2024/01/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugUiManager : DebugInputElement
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
        ShowAndHide = 1 << 3,
    }

    #endregion

    #region field

    /// <summary> UI�̕��ѕ��� </summary>
    [SerializeField, CustomLabel("UI�̕��ѕ���")]
    private UiOrder m_buttonOrder = UiOrder.Vertical;
    /// <summary> �J�[�\�������[�v�\�ɂ��邩 </summary>
    [SerializeField, CustomLabel("�J�[�\�������[�v�\�ɂ��邩")]
    private bool m_isLoop = false;
    ///// <summary> �{�^���̃v���n�u </summary>
    //[SerializeField, CustomLabel("�{�^���̃v���n�u")]
    //private GameObject m_buttonPrefab;
    /// <summary> �I����Ԃ̃{�^��Id </summary>
    [SerializeField, CustomLabelReadOnly("�I����Ԃ̃{�^��Id")]
    private int m_selectCursorIndex = 0;

    /// <summary> UI���X�i�[�X�N���v�g </summary>
    private UiListenerBase[] m_listener;
    /// <summary> ���͊m�F�֐�(�{�^���̕��т��Ƃɗp��) </summary>
    private System.Action[] m_checkInput;
    /// <summary> ���͊m�F�֐�(�{�^���̕��т��Ƃɗp��) </summary>
    private System.Action[] m_checkSlideInput;
    /// <summary> ����������͂̋����̂������l </summary>
    private const float m_inputMoveThreshold = 0.7f;
    /// <summary> �A�����͊J�n�҂����� </summary>
    private const float m_continueWaitTime = 0.5f;
    /// <summary> �A�����͑҂����� </summary>
    private const float m_continueIntervalTime = 0.1f;

    /// <summary> �C���v�b�g�A�N�V����(�J�[�\��) </summary>
    private NewInputAction_Vector2 m_inputAct_Cursor;
    /// <summary> �C���v�b�g�A�N�V����(�������) </summary>
    private NewInputAction_Button m_inputAct_Submit;
    /// <summary> �C���v�b�g�A�N�V����(�\���A��\������) </summary>
    private NewInputAction_Button m_inputAct_ShowAndHide_LeftStick;
    /// <summary> �C���v�b�g�A�N�V����(�\���A��\������) </summary>
    private NewInputAction_Button m_inputAct_ShowAndHide_RightStick;
    /// <summary> �X���C�_�[�̌��� </summary>
    private UiOrder m_sliderOrder = UiOrder.Vertical;
    /// <summary> �A�����͗p�X�e�[�g </summary>
    private InputState m_inputState = InputState.None;
    /// <summary> �A�����͗p�X�e�[�g </summary>
    private InputState m_slideinputState = InputState.None;
    /// <summary> �ǂ̃{�^����������Ă��邩�̃r�b�g�p�^�[�� </summary>
    private int m_inputPattern = (int)InputPattern.None;
    /// <summary> �X���C�h����łǂ̃{�^����������Ă��邩�̃r�b�g�p�^�[�� </summary>
    private int m_slideInputPattern = (int)InputPattern.None;
    /// <summary> �c�莞�� </summary>
    private float m_continueTimeCount = 0.0f;
    /// <summary> �c�莞�� </summary>
    private float m_slideContinueTimeCount = 0.0f;
    /// <summary> �N���㏉�߂ẴA�N�e�B�u��Ԃ� </summary>
    private bool m_isFirstEnable = true;
    /// <summary> �\�����邩 </summary>
    private bool m_isShow = false;

    #endregion


    private void OnEnable()
    {
        // �ŏ��̃A�N�e�B�u���̍ۂ͏��������Ԃɍ��킸�A
        // �Q�ƃG���[�ɂȂ�̂ł��Ȃ�
        if (m_isFirstEnable)
        {
            m_isFirstEnable = false;
        }
        else
        {
            // �{�^�������݂���ꍇ�͈�ԍŏ��̃{�^����I����Ԃɂ���
            Select(m_selectCursorIndex);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(transform.root.gameObject);

        //string[] names = System.Enum.GetNames(typeof(SceneNameEnum));

        //// �V�[���I���̃f�o�b�O���j���[���\�z
        //for (int i = 0; i < names.Length; i++)
        //{
        //    if (names[i] != "DebugMenu")
        //    {
        //        RectTransform trans = Instantiate(m_buttonPrefab, transform).GetComponent<RectTransform>();
        //        trans.GetComponent<SceneChangeCallback>().Init(names[i]);
        //        trans.anchoredPosition = new Vector2(0.0f, -50.0f) * (transform.childCount - 1);
        //    }
        //}
    }

    private void Start()
    {
        Hide();

        m_listener = new UiListenerBase[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            m_listener[i] = trans.GetComponent<UiListenerBase>();
            m_listener[i].InitIndex(i);
        }

        // �{�^�������݂���ꍇ�͈�ԍŏ��̃{�^����I����Ԃɂ���
        Select(m_selectCursorIndex);

        m_inputAct_Cursor = GetActionMap().GetAction_Vec2(NewInputActionName_Z_DebugMenu.Vector2.Cursor.ToString());
        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_Z_DebugMenu.Button.Submit.ToString());
        m_inputAct_ShowAndHide_LeftStick = GetActionMap().GetAction_Button(NewInputActionName_Z_DebugMenu.Button.ShowAndHide_LeftStick.ToString());
        m_inputAct_ShowAndHide_RightStick = GetActionMap().GetAction_Button(NewInputActionName_Z_DebugMenu.Button.ShowAndHide_RightStick.ToString());

        // �f���Q�[�g�Ɋ֐��ݒ�
        m_checkInput = new System.Action[(int)UiOrder.Length]
            { CheckInput_Vertical, CheckInput_Horizontal };
        m_checkSlideInput = new System.Action[(int)UiOrder.Length]
            { CheckSlideInput_Vertical, CheckSlideInput_Horizontal };

        // �{�^���̕����ɑ΂��ċt�ɂȂ�悤�ɐݒ�
        if (m_buttonOrder == UiOrder.Vertical)
        {
            m_sliderOrder = UiOrder.Horizontal;
        }
        else
        {
            m_sliderOrder = UiOrder.Vertical;
        }
    }

    protected override void Update()
    {
        base.Update();

        m_inputPattern = (int)InputPattern.None;
        m_slideInputPattern = (int)InputPattern.None;

        CheckInput_ShowAndHide();
        Execute_ShowAndHide();

        if (m_isShow)
        {
            // ���͎擾
            CheckInput_Main();
            // ���͂ɉ��������������s
            Execute_Main();
        }
    }

    /// <summary>
    /// ��ԍŏ��̗v�f��I��
    /// </summary>
    protected void FirstSelect()
    {
        // �{�^�������݂���ꍇ�͈�ԍŏ��̃{�^����I����Ԃɂ���
        Select(m_selectCursorIndex);
    }

    /// <summary>
    /// ���͊m�F
    /// </summary>
    private void CheckInput_Main()
    {
        m_checkInput[(int)m_buttonOrder]();
        CheckInput_Submit();
        m_checkSlideInput[(int)m_sliderOrder]();
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
    /// ���͊m�F(�c����)
    /// </summary>
    private void CheckInput_Vertical()
    {
        float vertical = m_inputAct_Cursor.GetVec2All().y;

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
    private void CheckInput_Horizontal()
    {
        float horizontal = m_inputAct_Cursor.GetVec2All().x;

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
    /// �X���C�h���͊m�F(�c����)
    /// </summary>
    private void CheckSlideInput_Vertical()
    {
        float vertical = m_inputAct_Cursor.GetVec2All().y;

        if (vertical >= m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Minus;
        }
        else if (vertical <= -m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Plus;
        }
    }

    /// <summary>
    /// �X���C�h���͊m�F(������)
    /// </summary>
    private void CheckSlideInput_Horizontal()
    {
        float horizontal = m_inputAct_Cursor.GetVec2All().x;

        if (horizontal <= -m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Minus;
        }
        else if (horizontal >= m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Plus;
        }
    }

    /// <summary>
    /// ���͊m�F(�\���A��\������)
    /// </summary>
    private void CheckInput_ShowAndHide()
    {
        if (m_inputAct_ShowAndHide_LeftStick.GetDownAll() &&
            m_inputAct_ShowAndHide_RightStick.GetDownAll())
        {
            m_inputPattern |= (int)InputPattern.ShowAndHide;
        }
    }

    /// <summary>
    /// ���͂ɉ������������s
    /// </summary>
    protected void Execute_Main()
    {
        Execute_Cursor();
        Execute_Submit();
        Execute_Slider();
    }

    private void Execute_Cursor()
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

    private void Execute_Submit()
    {
        // �������
        if ((m_inputPattern & (int)InputPattern.Submit) > 0)
        {
            // ���菈��
            Submit();
        }
    }

    private void Execute_Slider()
    {
        // �X���C�_�[�ړ�
        if ((m_slideInputPattern & (int)InputPattern.Plus) > 0 ||
            (m_slideInputPattern & (int)InputPattern.Minus) > 0)
        {
            // �J�[�\���ړ�
            switch (m_slideinputState)
            {
                case InputState.None:
                    // �A�����͑҂��ɂ���
                    m_slideinputState = InputState.Wait;
                    m_slideContinueTimeCount = m_continueWaitTime;

                    // �J�[�\���ړ�
                    MoveSlider();
                    break;
                case InputState.Wait:
                    if (m_slideContinueTimeCount <= 0.0f)
                    {
                        // �A�����͂��J�n����
                        m_slideinputState = InputState.Interval;
                        m_slideContinueTimeCount = m_continueIntervalTime;

                        // �J�[�\���ړ�
                        MoveSlider();
                    }
                    else
                    {
                        m_slideContinueTimeCount -= Time.deltaTime;
                    }
                    break;
                case InputState.Interval:
                    if (m_slideContinueTimeCount <= 0.0f)
                    {
                        m_slideContinueTimeCount = m_continueIntervalTime;

                        // �J�[�\���ړ�
                        MoveSlider();
                    }
                    else
                    {
                        m_slideContinueTimeCount -= Time.deltaTime;
                    }
                    break;
            }
        }
        else
        {
            // �J�[�\���ړ��̃{�^����������Ă��Ȃ��ꍇ�A�����͌n�p�����[�^��������
            m_slideContinueTimeCount = 0.0f;
            m_slideinputState = InputState.None;
        }
    }

    private void Execute_ShowAndHide()
    {
        // �\���A��\������
        if ((m_inputPattern & (int)InputPattern.ShowAndHide) > 0)
        {
            if (m_isShow)
            {
                // ��\������
                Hide();
            }
            else
            {
                // �\������
                Show();
            }
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
        }
        else if ((m_inputPattern & (int)InputPattern.Minus) > 0)
        {
            // ���[�v�s�ݒ�Ń��[�v���N����V�`���G�[�V�����̏ꍇ�͏��������Ȃ�
            if (!m_isLoop && m_selectCursorIndex == 0) { return; }

            Select((m_selectCursorIndex - 1 + transform.childCount) % transform.childCount);
        }
    }

    /// <summary>
    /// �I������
    /// </summary>
    public void Select(int index)
    {
        // ���݂̑I��������
        m_listener[m_selectCursorIndex].Unselect();
        m_selectCursorIndex = index;
        m_listener[m_selectCursorIndex].Select();
    }

    /// <summary>
    /// �\������
    /// </summary>
    public void Show()
    {
        m_isShow = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        Select(m_selectCursorIndex);
    }

    /// <summary>
    /// ��\������
    /// </summary>
    public void Hide()
    {
        m_isShow = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���菈��
    /// </summary>
    public void Submit()
    {
        m_listener[m_selectCursorIndex].Submit();
    }

    /// <summary>
    /// �J�[�\���ړ�
    /// </summary>
    private void MoveSlider()
    {
        if ((m_slideInputPattern & (int)InputPattern.Plus) > 0)
        {
            m_listener[m_selectCursorIndex].RightSlide();
        }
        else if ((m_slideInputPattern & (int)InputPattern.Minus) > 0)
        {
            m_listener[m_selectCursorIndex].LeftSlide();
        }
    }
}
