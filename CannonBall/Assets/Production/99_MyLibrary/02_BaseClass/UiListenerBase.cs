/*******************************************************************************
*
*	タイトル：	Uiリスナー基底クラス
*	ファイル：	UiListenerBase.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiListenerBase : MonoBehaviour
{
    public enum UiPattern
    {
        Select = 0,
        Button,
        RadioButton,
        Slider,
        SlideSwitch,
    }


    /// <summary> UIグループ上ID </summary>
    private int m_index;

    private UiPattern m_uiPattern;


    public int m_Index { get { return m_index; } }

    public UiPattern m_UiPattern { get { return m_uiPattern; } }

    /// <summary>
    /// UI種の設定
    /// </summary>
    protected void InitUiPattern(UiPattern pattern)
    {
        m_uiPattern = pattern;
    }

    /// <summary>
    /// IDの初期化
    /// </summary>
    public void InitIndex(int id)
    {
        m_index = id;
    }

    /// <summary>
    /// 自身を選択状態にする
    /// </summary>
    public virtual void Select() { }

    /// <summary>
    /// 自身を非選択状態にする
    /// </summary>
    public virtual void Unselect() { }

    /// <summary>
    /// 決定処理
    /// </summary>
    public virtual void Submit() { }

    /// <summary>
    /// キャンセル処理
    /// </summary>
    public virtual void Cancel() { }

    /// <summary>
    /// 左にスライド
    /// </summary>
    public virtual void LeftSlide() { }

    /// <summary>
    /// 左にスライド
    /// </summary>
    public virtual void RightSlide() { }

    /// <summary>
    /// スイッチの状態を変更
    /// </summary>
    protected virtual void ChangeSwitch() { }
}
