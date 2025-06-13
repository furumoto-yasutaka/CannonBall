/*******************************************************************************
*
*	タイトル：	ボタンリスナースクリプト
*	ファイル：	ButtonListener.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonListener : UiListenerBase
{
    /// <summary> ボタンが決定したときのコールバック </summary>
    [SerializeField]
    private UnityEvent m_onClickCallBack;

    public UnityEvent m_OnClickCallBack { get { return m_onClickCallBack; } }


    protected void Awake()
    {
        InitUiPattern(UiPattern.Button);
    }

    /// <summary>
    /// 決定処理
    /// </summary>
    public override void Submit()
    {
        m_onClickCallBack.Invoke();
    }
}
