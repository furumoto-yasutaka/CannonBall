using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyCheck : InputElement
{
    [SerializeField]
    bool m_AnyButtonTrigger = false;

    //TextAlpha m_textAlpha;
    [SerializeField]
    private TitleLogoSprite m_titleLogoSprite;

    private NewInputAction_Button m_inputAct;


    private void Start()
    {
        //m_textAlpha = GetComponent<TextAlpha>();

        m_inputAct = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.PressAnyBotton.ToString());

    }


    private void Update()
    {
        if (m_inputAct.GetDownAll()|| m_AnyButtonTrigger/*PressAnyŒŸ’m*/)
        {
            //m_textAlpha.AnyPressFinish();
            m_titleLogoSprite.StartAnimation();

            m_AnyButtonTrigger = false;
        }
    }

}
