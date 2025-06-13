using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageChange : InputElement
{
    [SerializeField]
    private SceneChanger m_sceneChanger;


    [Header("デバッグ用")]
    [SerializeField]
    bool m_isUpTrigger = false;
    [SerializeField]
    bool m_isDownTrigger = false;
    [SerializeField]
    bool m_isSumbmitTrigger = false;


    private NewInputAction_Button m_inputActUp;
    private NewInputAction_Button m_inputActDown;
    private NewInputAction_Button m_inputActSubmit;

    private void Start()
    {
        m_inputActUp = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorUp.ToString());
        m_inputActDown = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorDown.ToString());
        m_inputActSubmit = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorSubmit.ToString());

    }


    private void Update()
    {

        /*上の何かしらの入力を受け取る*/
        if (m_inputActUp.GetDownAll() || m_isUpTrigger) 
        {
            StageChangeManager.Instance.InputUp();
            m_isUpTrigger = false;
        }
        /*下の何かしらの入力を受け取る*/
        else if (m_inputActDown.GetDownAll() || m_isDownTrigger)
        {
            StageChangeManager.Instance.InputDown();
            m_isDownTrigger = false;
        }
        
        
        if (m_inputActSubmit.GetDownAll() || m_isSumbmitTrigger)
        {
            switch (StageChangeManager.Instance.GetCurrentStage())
            {
                case StageChangeManager.StageType.FightGame:

                    // 決定SE
                    AudioManager.Instance.PlaySe("決定ボタン", false);

                    m_sceneChanger.StartSceneChange(SceneNameEnum.TypeSelect);
                    break;
                case StageChangeManager.StageType.BombGame:

                    // 決定SE
                    AudioManager.Instance.PlaySe("決定ボタン", false);

                    m_sceneChanger.StartSceneChange(SceneNameEnum.BombRush);
                    break;
                case StageChangeManager.StageType.MagumaGame:

                    // 決定SE
                    AudioManager.Instance.PlaySe("決定ボタン", false);

                    m_sceneChanger.StartSceneChange(SceneNameEnum.DangerRun);
                    break;
            }
        }


    }

}
