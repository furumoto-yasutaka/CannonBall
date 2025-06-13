/*******************************************************************************
*
*	タイトル：	UIグループ制御スクリプト
*	ファイル：	UiSelectManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiSelectManager : InputElement
{
    #region enum

    /// <summary> UIの並び </summary>
    public enum UiOrder
    {
        Vertical = 0,
        Horizontal,
        Length,
    }

    /// <summary> 連続入力用ステート </summary>
    public enum InputState
    {
        None = 0,
        Wait,
        Interval,
    }

    /// <summary> 入力情報 </summary>
    public enum InputPattern
    {
        None = 0,
        Plus = 1 << 0,
        Minus = 1 << 1,
        Submit = 1 << 2,
    }

    #endregion

    #region field

    /// <summary> UIの並び方向 </summary>
    [SerializeField, CustomLabel("UIの並び方向")]
    protected UiOrder m_buttonOrder = UiOrder.Vertical;

    /// <summary> カーソルをループ可能にするか </summary>
    [SerializeField, CustomLabel("カーソルをループ可能にするか")]
    protected bool m_isLoop = false;

    /// <summary> UIリスナースクリプト </summary>
    protected UiListenerBase[] m_listener;
    /// <summary> 選択状態のボタンId </summary>
    public int m_selectCursorIndex = 0;

    /// <summary> 入力確認関数(ボタンの並びごとに用意) </summary>
    protected System.Action[] m_checkInput;
    /// <summary> 反応する入力の強さのしきい値 </summary>
    protected const float m_inputMoveThreshold = 0.7f;
    /// <summary> 連続入力開始待ち時間 </summary>
    protected const float m_continueWaitTime = 0.5f;
    /// <summary> 連続入力待ち時間 </summary>
    protected const float m_continueIntervalTime = 0.2f;

    /// <summary> インプットアクション(カーソル) </summary>
    protected NewInputAction_Vector2 m_inputAct_Cursor;
    /// <summary> 連続入力用ステート </summary>
    protected InputState m_inputState = InputState.None;
    /// <summary> どのボタンが押されているかのビットパターン </summary>
    protected int m_inputPattern = (int)InputPattern.None;
    /// <summary> 残り時間 </summary>
    protected float m_continueTimeCount = 0.0f;
    /// <summary> 起動後初めてのアクティブ状態か </summary>
    protected bool m_isFirstEnable = true;

    #endregion

    #region function

    protected virtual void Start()
    {
        InitListener();
        FirstSelect();

        m_inputAct_Cursor = GetActionMap().GetAction_Vec2(NewInputActionName_Ui.Vector2.Cursor.ToString());

        // デリゲートに関数設定
        m_checkInput = new System.Action[(int)UiOrder.Length]
            { CheckInput_Vertical, CheckInput_Horizontal };
    }

    protected virtual void OnEnable()
    {
        // 最初のアクティブ化の際は初期化が間に合わず、
        // 参照エラーになるのでしない
        if (m_isFirstEnable)
        {
            m_isFirstEnable = false;
        }
        else
        {
            // 最初の要素を選択する
            FirstSelect();
        }
    }

    protected virtual void Update()
    {
        if (!m_IsCanInput) { return; }

        // 入力取得
        CheckInput();
        // 入力に応じた処理を実行
        Execute();
    }

    /// <summary>
    /// 子オブジェクトからリスナーを取得し、初期化
    /// </summary>
    protected virtual void InitListener()
    {
        m_listener = new UiListenerBase[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            m_listener[i] = trans.GetComponent<UiListenerBase>();
            // IDを付与
            m_listener[i].InitIndex(i);
        }
    }

    /// <summary>
    /// 一番最初の要素を選択
    /// </summary>
    protected void FirstSelect()
    {
        if (transform.childCount == 0)
        {
            // ボタンが存在しない場合は入力を受け付けないようにする
            LockInput();
        }
        else
        {
            // ボタンが存在する場合は一番最初のボタンを選択状態にする
            Select(m_selectCursorIndex);
        }
    }

    /// <summary>
    /// 入力確認
    /// </summary>
    protected virtual void CheckInput()
    {
        m_checkInput[(int)m_buttonOrder]();
    }

    /// <summary>
    /// 入力確認(縦並び)
    /// </summary>
    protected void CheckInput_Vertical()
    {
        float vertical = m_inputAct_Cursor.GetVec2All().y;

        m_inputPattern = (int)InputPattern.None;

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
    /// 入力確認(横並び)
    /// </summary>
    protected void CheckInput_Horizontal()
    {
        float horizontal = m_inputAct_Cursor.GetVec2All().x;

        m_inputPattern = (int)InputPattern.None;

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
    /// 入力に応じた処理実行
    /// </summary>
    protected virtual void Execute()
    {
        if ((m_inputPattern & (int)InputPattern.Plus) > 0 ||
            (m_inputPattern & (int)InputPattern.Minus) > 0)
        {
            // カーソル移動
            switch (m_inputState)
            {
                case InputState.None:
                    // 連続入力待ちにする
                    m_inputState = InputState.Wait;
                    m_continueTimeCount = m_continueWaitTime;

                    // カーソル移動
                    MoveCursor();
                    break;
                case InputState.Wait:
                    if (m_continueTimeCount <= 0.0f)
                    {
                        // 連続入力を開始する
                        m_inputState = InputState.Interval;
                        m_continueTimeCount = m_continueIntervalTime;

                        // カーソル移動
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

                        // カーソル移動
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
            // カーソル移動のボタンが押されていない場合連続入力系パラメータを初期化
            m_continueTimeCount = 0.0f;
            m_inputState = InputState.None;
        }
    }

    /// <summary>
    /// カーソル移動
    /// </summary>
    protected void MoveCursor()
    {
        if ((m_inputPattern & (int)InputPattern.Plus) > 0)
        {
            // ループ不可設定でループが起こるシチュエーションの場合は処理をしない
            if (!m_isLoop && m_selectCursorIndex == transform.childCount - 1) { return; }

            Select((m_selectCursorIndex + 1) % transform.childCount);

            //// サウンド再生
            //AudioManager.Instance.PlaySe("アウトゲーム_カーソル移動音", false);
        }
        else if ((m_inputPattern & (int)InputPattern.Minus) > 0)
        {
            // ループ不可設定でループが起こるシチュエーションの場合は処理をしない
            if (!m_isLoop && m_selectCursorIndex == 0) { return; }

            Select((m_selectCursorIndex - 1 + transform.childCount) % transform.childCount);

            //// サウンド再生
            //AudioManager.Instance.PlaySe("アウトゲーム_カーソル移動音", false);
        }
    }

    /// <summary>
    /// 選択処理
    /// </summary>
    public virtual void Select(int index)
    {
        // 現在の選択を解除
        m_listener[m_selectCursorIndex].Unselect();
        m_selectCursorIndex = index;
        m_listener[m_selectCursorIndex].Select();
    }

    #endregion
}
