/*******************************************************************************
*
*	タイトル：	デバッグメニューUIグループ制御スクリプト
*	ファイル：	DebugUiManager.cs
*	作成者：	古本 泰隆
*	制作日：    2024/01/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugUiManager : DebugInputElement
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
        ShowAndHide = 1 << 3,
    }

    #endregion

    #region field

    /// <summary> UIの並び方向 </summary>
    [SerializeField, CustomLabel("UIの並び方向")]
    private UiOrder m_buttonOrder = UiOrder.Vertical;
    /// <summary> カーソルをループ可能にするか </summary>
    [SerializeField, CustomLabel("カーソルをループ可能にするか")]
    private bool m_isLoop = false;
    ///// <summary> ボタンのプレハブ </summary>
    //[SerializeField, CustomLabel("ボタンのプレハブ")]
    //private GameObject m_buttonPrefab;
    /// <summary> 選択状態のボタンId </summary>
    [SerializeField, CustomLabelReadOnly("選択状態のボタンId")]
    private int m_selectCursorIndex = 0;

    /// <summary> UIリスナースクリプト </summary>
    private UiListenerBase[] m_listener;
    /// <summary> 入力確認関数(ボタンの並びごとに用意) </summary>
    private System.Action[] m_checkInput;
    /// <summary> 入力確認関数(ボタンの並びごとに用意) </summary>
    private System.Action[] m_checkSlideInput;
    /// <summary> 反応する入力の強さのしきい値 </summary>
    private const float m_inputMoveThreshold = 0.7f;
    /// <summary> 連続入力開始待ち時間 </summary>
    private const float m_continueWaitTime = 0.5f;
    /// <summary> 連続入力待ち時間 </summary>
    private const float m_continueIntervalTime = 0.1f;

    /// <summary> インプットアクション(カーソル) </summary>
    private NewInputAction_Vector2 m_inputAct_Cursor;
    /// <summary> インプットアクション(決定入力) </summary>
    private NewInputAction_Button m_inputAct_Submit;
    /// <summary> インプットアクション(表示、非表示入力) </summary>
    private NewInputAction_Button m_inputAct_ShowAndHide_LeftStick;
    /// <summary> インプットアクション(表示、非表示入力) </summary>
    private NewInputAction_Button m_inputAct_ShowAndHide_RightStick;
    /// <summary> スライダーの向き </summary>
    private UiOrder m_sliderOrder = UiOrder.Vertical;
    /// <summary> 連続入力用ステート </summary>
    private InputState m_inputState = InputState.None;
    /// <summary> 連続入力用ステート </summary>
    private InputState m_slideinputState = InputState.None;
    /// <summary> どのボタンが押されているかのビットパターン </summary>
    private int m_inputPattern = (int)InputPattern.None;
    /// <summary> スライド操作でどのボタンが押されているかのビットパターン </summary>
    private int m_slideInputPattern = (int)InputPattern.None;
    /// <summary> 残り時間 </summary>
    private float m_continueTimeCount = 0.0f;
    /// <summary> 残り時間 </summary>
    private float m_slideContinueTimeCount = 0.0f;
    /// <summary> 起動後初めてのアクティブ状態か </summary>
    private bool m_isFirstEnable = true;
    /// <summary> 表示するか </summary>
    private bool m_isShow = false;

    #endregion


    private void OnEnable()
    {
        // 最初のアクティブ化の際は初期化が間に合わず、
        // 参照エラーになるのでしない
        if (m_isFirstEnable)
        {
            m_isFirstEnable = false;
        }
        else
        {
            // ボタンが存在する場合は一番最初のボタンを選択状態にする
            Select(m_selectCursorIndex);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(transform.root.gameObject);

        //string[] names = System.Enum.GetNames(typeof(SceneNameEnum));

        //// シーン選択のデバッグメニューを構築
        //for (int i = 0; i < names.Length; i++)
        //{
        //    if (names[i] != "DebugMenu")
        //    {
        //        RectTransform trans = Instantiate(m_buttonPrefab, transform).GetComponent<RectTransform>();
        //        trans.GetComponent<SceneChangeCallback>().Init(names[i]);
        //        trans.anchoredPosition = new Vector2(0.0f, -50.0f) * (transform.childCount - 1);
        //    }
        //}
    }

    private void Start()
    {
        Hide();

        m_listener = new UiListenerBase[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            m_listener[i] = trans.GetComponent<UiListenerBase>();
            m_listener[i].InitIndex(i);
        }

        // ボタンが存在する場合は一番最初のボタンを選択状態にする
        Select(m_selectCursorIndex);

        m_inputAct_Cursor = GetActionMap().GetAction_Vec2(NewInputActionName_Z_DebugMenu.Vector2.Cursor.ToString());
        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_Z_DebugMenu.Button.Submit.ToString());
        m_inputAct_ShowAndHide_LeftStick = GetActionMap().GetAction_Button(NewInputActionName_Z_DebugMenu.Button.ShowAndHide_LeftStick.ToString());
        m_inputAct_ShowAndHide_RightStick = GetActionMap().GetAction_Button(NewInputActionName_Z_DebugMenu.Button.ShowAndHide_RightStick.ToString());

        // デリゲートに関数設定
        m_checkInput = new System.Action[(int)UiOrder.Length]
            { CheckInput_Vertical, CheckInput_Horizontal };
        m_checkSlideInput = new System.Action[(int)UiOrder.Length]
            { CheckSlideInput_Vertical, CheckSlideInput_Horizontal };

        // ボタンの方向に対して逆になるように設定
        if (m_buttonOrder == UiOrder.Vertical)
        {
            m_sliderOrder = UiOrder.Horizontal;
        }
        else
        {
            m_sliderOrder = UiOrder.Vertical;
        }
    }

    protected override void Update()
    {
        base.Update();

        m_inputPattern = (int)InputPattern.None;
        m_slideInputPattern = (int)InputPattern.None;

        CheckInput_ShowAndHide();
        Execute_ShowAndHide();

        if (m_isShow)
        {
            // 入力取得
            CheckInput_Main();
            // 入力に応じた処理を実行
            Execute_Main();
        }
    }

    /// <summary>
    /// 一番最初の要素を選択
    /// </summary>
    protected void FirstSelect()
    {
        // ボタンが存在する場合は一番最初のボタンを選択状態にする
        Select(m_selectCursorIndex);
    }

    /// <summary>
    /// 入力確認
    /// </summary>
    private void CheckInput_Main()
    {
        m_checkInput[(int)m_buttonOrder]();
        CheckInput_Submit();
        m_checkSlideInput[(int)m_sliderOrder]();
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
    /// 入力確認(縦並び)
    /// </summary>
    private void CheckInput_Vertical()
    {
        float vertical = m_inputAct_Cursor.GetVec2All().y;

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
    private void CheckInput_Horizontal()
    {
        float horizontal = m_inputAct_Cursor.GetVec2All().x;

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
    /// スライド入力確認(縦並び)
    /// </summary>
    private void CheckSlideInput_Vertical()
    {
        float vertical = m_inputAct_Cursor.GetVec2All().y;

        if (vertical >= m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Minus;
        }
        else if (vertical <= -m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Plus;
        }
    }

    /// <summary>
    /// スライド入力確認(横並び)
    /// </summary>
    private void CheckSlideInput_Horizontal()
    {
        float horizontal = m_inputAct_Cursor.GetVec2All().x;

        if (horizontal <= -m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Minus;
        }
        else if (horizontal >= m_inputMoveThreshold)
        {
            m_slideInputPattern |= (int)InputPattern.Plus;
        }
    }

    /// <summary>
    /// 入力確認(表示、非表示入力)
    /// </summary>
    private void CheckInput_ShowAndHide()
    {
        if (m_inputAct_ShowAndHide_LeftStick.GetDownAll() &&
            m_inputAct_ShowAndHide_RightStick.GetDownAll())
        {
            m_inputPattern |= (int)InputPattern.ShowAndHide;
        }
    }

    /// <summary>
    /// 入力に応じた処理実行
    /// </summary>
    protected void Execute_Main()
    {
        Execute_Cursor();
        Execute_Submit();
        Execute_Slider();
    }

    private void Execute_Cursor()
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

    private void Execute_Submit()
    {
        // 決定入力
        if ((m_inputPattern & (int)InputPattern.Submit) > 0)
        {
            // 決定処理
            Submit();
        }
    }

    private void Execute_Slider()
    {
        // スライダー移動
        if ((m_slideInputPattern & (int)InputPattern.Plus) > 0 ||
            (m_slideInputPattern & (int)InputPattern.Minus) > 0)
        {
            // カーソル移動
            switch (m_slideinputState)
            {
                case InputState.None:
                    // 連続入力待ちにする
                    m_slideinputState = InputState.Wait;
                    m_slideContinueTimeCount = m_continueWaitTime;

                    // カーソル移動
                    MoveSlider();
                    break;
                case InputState.Wait:
                    if (m_slideContinueTimeCount <= 0.0f)
                    {
                        // 連続入力を開始する
                        m_slideinputState = InputState.Interval;
                        m_slideContinueTimeCount = m_continueIntervalTime;

                        // カーソル移動
                        MoveSlider();
                    }
                    else
                    {
                        m_slideContinueTimeCount -= Time.deltaTime;
                    }
                    break;
                case InputState.Interval:
                    if (m_slideContinueTimeCount <= 0.0f)
                    {
                        m_slideContinueTimeCount = m_continueIntervalTime;

                        // カーソル移動
                        MoveSlider();
                    }
                    else
                    {
                        m_slideContinueTimeCount -= Time.deltaTime;
                    }
                    break;
            }
        }
        else
        {
            // カーソル移動のボタンが押されていない場合連続入力系パラメータを初期化
            m_slideContinueTimeCount = 0.0f;
            m_slideinputState = InputState.None;
        }
    }

    private void Execute_ShowAndHide()
    {
        // 表示、非表示入力
        if ((m_inputPattern & (int)InputPattern.ShowAndHide) > 0)
        {
            if (m_isShow)
            {
                // 非表示処理
                Hide();
            }
            else
            {
                // 表示処理
                Show();
            }
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
        }
        else if ((m_inputPattern & (int)InputPattern.Minus) > 0)
        {
            // ループ不可設定でループが起こるシチュエーションの場合は処理をしない
            if (!m_isLoop && m_selectCursorIndex == 0) { return; }

            Select((m_selectCursorIndex - 1 + transform.childCount) % transform.childCount);
        }
    }

    /// <summary>
    /// 選択処理
    /// </summary>
    public void Select(int index)
    {
        // 現在の選択を解除
        m_listener[m_selectCursorIndex].Unselect();
        m_selectCursorIndex = index;
        m_listener[m_selectCursorIndex].Select();
    }

    /// <summary>
    /// 表示処理
    /// </summary>
    public void Show()
    {
        m_isShow = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        Select(m_selectCursorIndex);
    }

    /// <summary>
    /// 非表示処理
    /// </summary>
    public void Hide()
    {
        m_isShow = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 決定処理
    /// </summary>
    public void Submit()
    {
        m_listener[m_selectCursorIndex].Submit();
    }

    /// <summary>
    /// カーソル移動
    /// </summary>
    private void MoveSlider()
    {
        if ((m_slideInputPattern & (int)InputPattern.Plus) > 0)
        {
            m_listener[m_selectCursorIndex].RightSlide();
        }
        else if ((m_slideInputPattern & (int)InputPattern.Minus) > 0)
        {
            m_listener[m_selectCursorIndex].LeftSlide();
        }
    }
}
