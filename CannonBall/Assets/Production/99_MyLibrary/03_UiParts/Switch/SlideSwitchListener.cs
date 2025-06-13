/*******************************************************************************
*
*	タイトル：	スライド式スイッチリスナースクリプト
*	ファイル：	SlideSwitchListener.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSwitchListener : UiListenerBase
{
    /// <summary> スイッチが動くアニメーション </summary>
    [SerializeField, CustomLabel("スイッチが動くアニメーション")]
    protected Animator m_switchAnimator = null;

    /// <summary> スイッチはオンか </summary>
    [CustomReadOnly]
    public bool m_IsOn = true;


    protected void Awake()
    {
        InitUiPattern(UiPattern.SlideSwitch);
    }

    /// <summary>
    /// 左にスイッチを切り替え
    /// </summary>
    public override void LeftSlide()
    {
        if (!m_IsOn)
        {
            ChangeSwitch();
        }
    }

    /// <summary>
    /// 右にスイッチを切り替え
    /// </summary>
    public override void RightSlide()
    {
        if (m_IsOn)
        {
            ChangeSwitch();
        }
    }

    /// <summary>
    /// スイッチの状態を変更
    /// </summary>
    protected override void ChangeSwitch()
    {
        m_IsOn = !m_IsOn;
        SetAnimation();
    }

    /// <summary>
    /// アニメーション設定
    /// </summary>
    protected void SetAnimation()
    {
        if (m_switchAnimator != null)
        {
            m_switchAnimator.SetBool("IsOn", m_IsOn);
        }
    }
}
