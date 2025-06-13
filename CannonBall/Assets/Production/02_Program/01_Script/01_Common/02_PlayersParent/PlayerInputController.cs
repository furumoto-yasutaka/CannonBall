using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : InputElement
{
    /// <summary> インプットアクション(移動入力) </summary>
    protected NewInputAction_Vector2 m_inputAct_Move;
    /// <summary> インプットアクション(蹴りの瞬間 ※Padスティック) </summary>
    protected NewInputAction_Button m_inputAct_Kick;
    /// <summary> インプットアクション(蹴りの方向 ※Padスティック) </summary>
    protected NewInputAction_Vector2 m_inputAct_KickDir;


    protected virtual void Awake()
    {
        m_inputAct_Move = GetActionMap().GetAction_Vec2(NewInputActionName_Player.Vector2.Move.ToString());
        m_inputAct_Kick = GetActionMap().GetAction_Button(NewInputActionName_Player.Button.Kick_Pad.ToString());
        m_inputAct_KickDir = GetActionMap().GetAction_Vec2(NewInputActionName_Player.Vector2.KickDir_Pad.ToString());
    }

    public Vector2 GetMove(int id)
    {
        return m_inputAct_Move.GetVec2(id);
    }

    public bool GetKick(int id)
    {
        return m_inputAct_Kick.GetDown(id);
    }

    public Vector2 GetKickDir(int id)
    {
        return m_inputAct_KickDir.GetVec2(id);
    }
}
