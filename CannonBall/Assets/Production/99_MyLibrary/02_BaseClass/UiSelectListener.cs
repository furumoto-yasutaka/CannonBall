/*******************************************************************************
*
*	タイトル：	Uiリスナー基底クラス
*	ファイル：	UiSelectListener.cs
*	作成者：	古本 泰隆
*	制作日：    2024/01/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSelectListener : UiListenerBase
{
    /// <summary> 選択時用アニメーター </summary>
    protected Animator m_animator;


    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
        InitUiPattern(UiPattern.Select);
    }

    /// <summary>
    /// 自身を選択状態にする
    /// </summary>
    public override void Select()
    {
        m_animator.SetBool("IsSelect", true);
    }

    /// <summary>
    /// 自身を非選択状態にする
    /// </summary>
    public override void Unselect()
    {
        m_animator.SetBool("IsSelect", false);
    }
}
