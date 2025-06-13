/*******************************************************************************
*
*	タイトル：	ラジオボタングループ制御スクリプト(汎用)
*	ファイル：	RadioButtonSelectManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioButtonSelectManager : UiSelectManager
{
    /// <summary> 決定状態にあるUI </summary>
    protected int m_submitIndex = 0;

    /// <summary> インプットアクション(決定入力) </summary>
    private NewInputAction_Button m_inputAct_Submit;


    protected override void Start()
    {
        base.Start();

        InitSubmitIndex();

        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_Ui.Button.Submit.ToString());
    }

    /// <summary>
    /// 決定状態の初期化
    /// </summary>
    private void InitSubmitIndex()
    {
        // ★セーブデータから取得
        //m_submitIndex = SaveDataManager.Instance.m_SaveData.Value.Option.KeyConfigPattern;
        ((RadioButtonListener)m_listener[m_submitIndex]).Submit();
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
            m_listener[i] = trans.GetComponent<RadioButtonListener>();
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
    public virtual void Submit()
    {
        ((RadioButtonListener)m_listener[m_submitIndex]).Cancel();

        m_submitIndex = m_selectCursorIndex;

        ((RadioButtonListener)m_listener[m_selectCursorIndex]).Submit();

        // サウンド再生
        AudioManager.Instance.PlaySe("アウトゲーム_決定音", false);
    }
}
