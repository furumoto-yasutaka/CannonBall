using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeSelectWindow : Window
{
    [System.Serializable]
    public struct Controllers
    {
        public TypePanelController PanelController;
        public SubmitIconController SubmitIconController;
        public RectTransform Triangle;
        public RectTransform SubmitOparation;
        public RectTransform CancelOparation;
        public DetailController DetailController;
    }


    [SerializeField, CustomLabel("�p�l���̐���R���|�[�l���g")]
    private Controllers[] m_controllers;

    [SerializeField, CustomLabel("�m�F��ʐ���R���|�[�l���g")]
    private ConfirmController m_confirmController;

    [SerializeField, CustomLabel("�I����ʂ��烂�[�h�I���֖߂�̂ɂ����鎞��")]
    private float m_backPressTime = 3.0f;

    [SerializeField, CustomLabel("���[�h�I���ɖ߂�{�^�������������Ԃ������摜")]
    private Image m_backPressProgressImage;

    /// <summary> �C���v�b�g�A�N�V����(�J�[�\����) </summary>
    private NewInputAction_Button m_inputAct_Cursor_Left;
    /// <summary> �C���v�b�g�A�N�V����(�J�[�\���E) </summary>
    private NewInputAction_Button m_inputAct_Cursor_Right;
    /// <summary> �C���v�b�g�A�N�V����(����) </summary>
    private NewInputAction_Button m_inputAct_Submit;
    /// <summary> �C���v�b�g�A�N�V����(�߂�) </summary>
    private NewInputAction_Button m_inputAct_Cancel;
    /// <summary> �C���v�b�g�A�N�V����(�ڍו\��) </summary>
    private NewInputAction_Button m_inputAct_Detail;
    /// <summary> �C���v�b�g�A�N�V����(�R��̏u�� ��Pad�X�e�B�b�N) </summary>
    private NewInputAction_Button m_inputAct_Kick_Pad;
    /// <summary> �C���v�b�g�A�N�V����(�R��̕��� ��Pad�X�e�B�b�N) </summary>
    private NewInputAction_Vector2 m_inputAct_KickDir_Pad;

    private bool m_isSubmitAll = false;

    private bool m_isSceneEnd = false;

    private float m_backPressTimeCount;


    private void Start()
    {
        m_inputAct_Cursor_Left = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Cursor_Left.ToString());
        m_inputAct_Cursor_Right = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Cursor_Right.ToString());
        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Submit.ToString());
        m_inputAct_Cancel = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Cancel.ToString());
        m_inputAct_Detail = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Detail.ToString());
        m_inputAct_Kick_Pad = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Kick_Pad.ToString());
        m_inputAct_KickDir_Pad = GetActionMap().GetAction_Vec2(NewInputActionName_TypeSelect.Vector2.KickDir_Pad.ToString());

        m_backPressTimeCount = m_backPressTime;
    }

    private void Update()
    {
        if (m_isSceneEnd) { return; }

        if (m_isSubmitAll)
        {
            ConfirmUpdate();
        }
        else
        {
            SelectUpdate();
        }
    }

    private void SelectUpdate()
    {
        bool isAllSubmit = true;

        for (int i = 0; i < 4; i++)
        {
            if (!m_controllers[i].SubmitIconController.m_IsSubmit)
            {
                // �J�[�\���ړ�
                if (m_inputAct_Cursor_Left.GetPress(i))
                {
                    m_controllers[i].PanelController.LeftCursor();
                }
                else if (m_inputAct_Cursor_Right.GetPress(i))
                {
                    m_controllers[i].PanelController.RightCursor();
                }

                // �������
                if (m_inputAct_Submit.GetDown(i))
                {
                    m_controllers[i].SubmitIconController.Submit(i);
                    SubmitSetScale(i);
                    AudioManager.Instance.PlaySe("�^�C�v�I��_����", false);
                    // �R���g���[���[�U��
                    VibrationManager.Instance.SetVibration(i, 10, 0.7f);
                }
            }
            else
            {
                // �������
                if (m_inputAct_Submit.GetDown(i))
                {
                    m_controllers[i].SubmitIconController.Cancel();
                    CancelSetScale(i);
                    AudioManager.Instance.PlaySe("�^�C�v�I��_����L�����Z��", false);
                }
            }

            // �ڍו\��
            if (m_inputAct_Detail.GetDown(i))
            {
                if (m_controllers[i].DetailController.m_IsShow)
                {
                    m_controllers[i].DetailController.Hide();
                    AudioManager.Instance.PlaySe("�^�C�v�I��_����L�����Z��", false);
                }
                else
                {
                    m_controllers[i].DetailController.Show();
                    AudioManager.Instance.PlaySe("�^�C�v�I��_�ڍ׃E�B���h�E�\��", false);
                }
            }

            if (!m_controllers[i].SubmitIconController.m_IsSubmit)
            {
                isAllSubmit = false;
            }
        }

        // �S���������ԂɂȂ�����m�F��ʂ��o��
        if (isAllSubmit)
        {
            m_isSubmitAll = true;
            m_confirmController.Show();

            // �I����ʂ��I���̂ŃJ�E���g�����Z�b�g����
            m_backPressTimeCount = m_backPressTime;
            m_backPressProgressImage.fillAmount = 0.0f;
        }
        else if (m_inputAct_Cancel.GetPressAll())
        {
            m_backPressTimeCount -= Time.deltaTime;

            if (m_backPressTimeCount < 0.0f)
            {
                // �O�̃V�[���ɖ߂�
                GetComponent<SceneChanger>().StartSceneChange();

                m_isSceneEnd = true;
            }
            else
            {
                m_backPressProgressImage.fillAmount = m_backPressTimeCount / m_backPressTime - 0.01f;
            }
        }
        else
        {
            m_backPressTimeCount = m_backPressTime;
            m_backPressProgressImage.fillAmount = 0.0f;
        }
    }

    private void ConfirmUpdate()
    {
        if (m_inputAct_Submit.GetDownAll())
        {
            for (int i = 0; i < 4; i++)
            {
                CreatePlayer.SetInitType(i, (PlayerController.Type)m_controllers[i].PanelController.m_CursorNum);
            }

            m_confirmController.Submit();
            AudioManager.Instance.PlaySe("�^�C�v�I��_�ŏI�m�F", false);

            // �R���g���[���[�U��
            VibrationManager.Instance.SetVibrationAll(10, 0.9f);

            m_isSceneEnd = true;
        }
        else if (m_inputAct_Cancel.GetDownAll())
        {
            // �m�F��ʂ���I����ʂɖ߂�
            m_isSubmitAll = false;
            m_confirmController.Hide();
            AudioManager.Instance.PlaySe("�^�C�v�I��_����L�����Z��", false);

            // �S���̌����Ԃ���������
            for (int i = 0; i < 4; i++)
            {
                m_controllers[i].SubmitIconController.Cancel();
                CancelSetScale(i);
            }
        }
    }

    private void SubmitSetScale(int i)
    {
        m_controllers[i].Triangle.localScale = Vector3.zero;
        m_controllers[i].SubmitOparation.localScale = Vector3.zero;
        m_controllers[i].CancelOparation.localScale = Vector3.one;
    }

    private void CancelSetScale(int i)
    {
        m_controllers[i].Triangle.localScale = Vector3.one;
        m_controllers[i].SubmitOparation.localScale = Vector3.one;
        m_controllers[i].CancelOparation.localScale = Vector3.zero;
    }
}
