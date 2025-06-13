/*******************************************************************************
*
*	タイトル：	ラジオボタンリスナースクリプト
*	ファイル：	RadioButtonListener.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButtonListener : UiListenerBase
{
    /// <summary> 選択していることを表す画像 </summary>
    [SerializeField, CustomLabel("選択していることを表す画像")]
    private GameObject m_submitImage;


    protected void Awake()
    {
        InitUiPattern(UiPattern.RadioButton);
    }

    /// <summary>
    /// 決定処理
    /// </summary>
    public override void Submit()
    {
        m_submitImage.SetActive(true);
    }

    /// <summary>
    /// キャンセル処理
    /// </summary>
    public override void Cancel()
    {
        m_submitImage.SetActive(false);
    }
}
