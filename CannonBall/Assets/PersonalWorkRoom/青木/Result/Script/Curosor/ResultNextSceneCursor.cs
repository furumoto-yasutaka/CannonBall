using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultNextSceneCursor : InputElement
{
    public enum CURSOR_TYPE
    {
        TO_STAGESELECT,
        TO_TITLE,
    }


    [SerializeField]
    GameObject m_toStageSelect;

    [SerializeField]
    GameObject m_toTitle;


    private NewInputAction_Button m_inputUpAct;
    private NewInputAction_Button m_inputDownAct;

    private CURSOR_TYPE m_cursorType = CURSOR_TYPE.TO_STAGESELECT;

    public CURSOR_TYPE GetCursorType() { return m_cursorType; }

    private void Start()
    {
        m_inputUpAct = GetActionMap().GetAction_Button(NewInputActionName_ResultPlayer.Button.CursorUp.ToString());
        m_inputDownAct = GetActionMap().GetAction_Button(NewInputActionName_ResultPlayer.Button.CursorDown.ToString());
    }

    private void Update()
    {
        if (m_inputUpAct.GetDownAll())
        {
            transform.position = m_toStageSelect.transform.position;

            m_cursorType = CURSOR_TYPE.TO_STAGESELECT;
        }
        if (m_inputDownAct.GetDownAll())
        {
            transform.position = m_toTitle.transform.position;

            m_cursorType = CURSOR_TYPE.TO_TITLE;
        }
    }


}
