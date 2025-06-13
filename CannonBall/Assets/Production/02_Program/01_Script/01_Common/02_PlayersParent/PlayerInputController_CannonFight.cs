using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController_CannonFight : PlayerInputController
{
    /// <summary> インプットアクション(必殺技発動) </summary>
    protected NewInputAction_Button m_inputAct_SpMove;


    protected override void Awake()
    {
        base.Awake();
        m_inputAct_SpMove = GetActionMap().GetAction_Button(NewInputActionName_Player.Button.SpMove.ToString());
    }

    public bool GetSpMove(int id)
    {
        return m_inputAct_SpMove.GetDown(id);
    }
}
