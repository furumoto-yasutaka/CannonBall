/*******************************************************************************
*
*	タイトル：	ボタングループ制御スクリプト(汎用)
*	ファイル：	ButtonSelectManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonSelectManager : UiSelectManager
{
    /// <summary> インプットアクション(決定入力) </summary>
    private NewInputAction_Button m_inputAct_Submit;


    protected override void Start()
    {
        base.Start();

        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_Ui.Button.Submit.ToString());
    }

    /// <summary>
    /// 子オブジェクトからリスナーを取得し、初期化
    /// </summary>
    protected override void InitListener()
    {
        m_listener = new UiListenerBase[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            m_listener[i] = trans.GetComponent<ButtonListener>();
            m_listener[i].InitIndex(i);
        }
    }

    /// <summary>
    /// 入力確認
    /// </summary>
    protected override void CheckInput()
    {
        base.CheckInput();
        CheckInput_Submit();
    }

    /// <summary>
    /// 入力確認(決定入力)
    /// </summary>
    private void CheckInput_Submit()
    {
        if (m_inputAct_Submit.GetDownAll())
        {
            m_inputPattern |= (int)InputPattern.Submit;
        }
    }

    /// <summary>
    /// 入力に応じた処理実行
    /// </summary>
    protected override void Execute()
    {
        base.Execute();

        if ((m_inputPattern & (int)InputPattern.Submit) > 0)
        {
            // 決定処理
            Submit();
        }
    }

    /// <summary>
    /// 決定処理
    /// </summary>
    public void Submit()
    {
        ((ButtonListener)m_listener[m_selectCursorIndex]).Submit();
    }
}
