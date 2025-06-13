using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuideCursorInput : InputElement
{
    public enum STATE
    {
        NONE,

        LOGO,
        PLAY_GUIDE,
    }

    [SerializeField, CustomReadOnly]
    STATE m_state = STATE.LOGO;

    [SerializeField, CustomLabel("�E���")]
    CommonSlideArrowView m_rightArrowView;

    [SerializeField, CustomLabel("�����")]
    CommonSlideArrowView m_leftArrowView;


    private NewInputAction_Button m_inputRightAct;
    private NewInputAction_Button m_inputLeftAct;


    Animator[] m_animator;

    PlayGuideCursor[] m_playGuideCursors;

    public void SetState(STATE _state) { m_state = _state; }
    public STATE GetState() { return m_state;}


    private void Start()
    {
        // �J�[�\��
        m_playGuideCursors = new PlayGuideCursor[transform.childCount];
        for (int i = 0; i < m_playGuideCursors.Length; i++) 
        {
            m_playGuideCursors[i] = transform.GetChild(i).GetComponent<PlayGuideCursor>();
        }

        // �A�j���[�V����
        m_animator = new Animator[transform.childCount];
        for (int i = 0; i < m_animator.Length; i++)
        {
            m_animator[i] = transform.GetChild(i).GetComponent<Animator>();
        }



        // ���͐ݒ�
        m_inputRightAct = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorRight.ToString());
        m_inputLeftAct = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorLeft.ToString());


    }



    private void Update()
    {
        int currentAnimIndex = (int)StageChangeManager.Instance.GetCurrentStage();

        // ������@�ɑJ��
        if (m_inputRightAct.GetDownAll())
        {
            m_state = STATE.PLAY_GUIDE;
            m_animator[currentAnimIndex].SetInteger("State", (int)m_state);

            if (m_rightArrowView.GetState() != CommonSlideArrowView.STATE.HIDE &&
                m_leftArrowView.GetState() != CommonSlideArrowView.STATE.VIEW)
            {
                AudioManager.Instance.PlaySe("���[����ʈړ�", false);
            }

            m_rightArrowView.SetState(CommonSlideArrowView.STATE.HIDE);
            m_leftArrowView.SetState(CommonSlideArrowView.STATE.VIEW);
        }
        // ���S�ɑJ��
        if (m_inputLeftAct.GetDownAll() && !m_animator[currentAnimIndex].GetCurrentAnimatorStateInfo(0).IsName("None"))
        {
            m_state = STATE.LOGO;
            m_animator[currentAnimIndex].SetInteger("State", (int)m_state);

            if (m_rightArrowView.GetState() != CommonSlideArrowView.STATE.VIEW &&
                m_leftArrowView.GetState() != CommonSlideArrowView.STATE.HIDE)
            {
                AudioManager.Instance.PlaySe("���[����ʈړ�", false);
            }

            m_rightArrowView.SetState(CommonSlideArrowView.STATE.VIEW);
            m_leftArrowView.SetState(CommonSlideArrowView.STATE.HIDE);
        }

    }



}
