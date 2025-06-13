/*******************************************************************************
*
*	タイトル：	ウィンドウ制御スクリプト
*	ファイル：	Window.cs
*	作成者：	古本 泰隆
*	制作日：    2023/04/16
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Window : InputElement
{
    /// <summary> ウィンドウ表示・非表示時用のアニメーション </summary>
    private Animator m_animator;

    /// <summary> 1つ前のウィンドウ(前のウィンドウへ戻る際に使用) </summary>
    private Window m_beforeWindow;

    /// <summary> 画面遷移が可能な状態か </summary>
    private bool m_isCanTransition = true;

    /// <summary> インプットアクションマップ </summary>
    protected NewInputActionMap m_inputActionMap;

    public bool m_IsCanTransition
    {
        get { return m_isCanTransition; }
    }

    public NewInputActionMap m_InputActionMap
    {
        get { return m_inputActionMap; }
        set { m_inputActionMap = value; }
    }


    protected virtual void Awake()
    {
        TryGetComponent(out m_animator);
    }

    /// <summary>
    /// インプットアクションマップの初期化
    /// </summary>
    public void InitInputActionMap(NewInputActionMap map)
    {
        m_inputActionMap = map;
    }

    /// <summary>
    /// ウィンドウを開く
    /// </summary>
    /// <param name="window"> 遷移元ウィンドウ(遷移元ウィンドウが閉じたことで遷移した場合は必要なし) </param>
    /// <param name="isUnlockGroup"> グループの入力ロックを解除するか </param>
    protected virtual void Open(Window window = null, bool isUnlockGroup = true)
    {
        // 遷移元ウィンドウが渡されている場合のみ設定
        if (window != null)
        {
            m_beforeWindow = window;
        }

        // アニメーターの設定
        if (m_animator != null)
        {
            m_animator.SetBool("IsOpen", true);
            m_isCanTransition = true;
        }
        gameObject.SetActive(true);

        // グループの入力ロックの解除
        if (isUnlockGroup)
        {
            m_Manager.UnlockInputGroupOrder(m_Group, false);
        }
    }

    /// <summary>
    /// ウィンドウを閉じる
    /// </summary>
    /// <param name="isLockGroup"> グループの入力ロックを解除するか </param>
    protected virtual void Close(bool isLockGroup = true)
    {
        // アニメーターの設定
        if (m_animator != null)
        {
            m_animator.SetBool("IsOpen", false);
            m_isCanTransition = false;
        }
        else
        {
            gameObject.SetActive(false);
        }

        // グループの入力をロック
        if (isLockGroup)
        {
            m_Manager.LockInputGroupOrder(false);
        }
    }

    /// <summary>
    /// 指定したウィンドウに遷移
    /// </summary>
    /// <param name="window"> 遷移先ウィンドウ </param>
    /// <returns> 遷移が実行されたか </returns>
    protected virtual bool NextWindowChange(Window window)
    {
        bool isTransition = m_isCanTransition && window.m_IsCanTransition;

        if (isTransition)
        {
            Close();
            window.Open(this);
        }

        return isTransition;
    }

    /// <summary>
    /// 指定したポップアップウィンドウを表示
    /// </summary>
    /// <param name="window"> 表示ウィンドウ </param>
    /// <returns> 遷移が実行されたか </returns>
    protected virtual bool PopupWindowOpen(Window window)
    {
        bool isTransition = m_isCanTransition && window.m_IsCanTransition;

        if (m_isCanTransition && window.m_IsCanTransition)
        {
            m_Manager.LockInputGroupOrder(false);
            window.Open(this);

            // サウンド再生
            AudioManager.Instance.PlaySe("アウトゲーム_ポップアップウィンドウ表示音", false);
        }

        return isTransition;
    }

    /// <summary>
    /// 1つ前のウィンドウに戻る
    /// </summary>
    /// <returns> 遷移が実行されたか </returns>
    protected virtual bool BeforeWindowChange()
    {
        bool isTransition = m_isCanTransition;

        if (isTransition)
        {
            Close();
            m_beforeWindow.Open();
        }

        return isTransition;
    }

    /// <summary>
    /// 閉じる処理の終了時コールバック関数
    /// </summary>
    public virtual void OpenComplete()
    {
        m_isCanTransition = true;
    }

    /// <summary>
    /// 閉じる処理の終了時コールバック関数
    /// </summary>
    public virtual void CloseComplete()
    {
        m_isCanTransition = true;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// アクションマップを有効化
    /// </summary>
    public virtual void EnableInputActionMap()
    {
        m_inputActionMap.Enable();
    }

    /// <summary>
    /// アクションマップを無効化
    /// </summary>
    public virtual void DisableInputActionMap()
    {
        m_inputActionMap.Disable();
    }
}
