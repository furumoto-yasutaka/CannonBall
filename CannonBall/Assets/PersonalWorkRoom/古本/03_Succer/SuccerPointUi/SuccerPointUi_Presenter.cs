/*******************************************************************************
*
*	タイトル：	サッカーの得点表示制御仲介スクリプト
*	ファイル：	SuccerPointUi_Presenter.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SuccerPointUi_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> ポイント表示(赤チーム) </summary>
    [SerializeField, CustomLabel("ポイント表示(赤チーム)")]
    private SuccerPointUi_View m_viewRed;

    /// <summary> ポイント表示(青チーム) </summary>
    [SerializeField, CustomLabel("ポイント表示(青チーム)")]
    private SuccerPointUi_View m_viewBlue;


    void Start()
    {
        SuccerTeamPointManager.Instance.m_RedPoint.Subscribe(v =>
            {
                m_viewRed.SetValue(v);
            })
            .AddTo(this);

        SuccerTeamPointManager.Instance.m_BluePoint.Subscribe(v =>
            {
                m_viewBlue.SetValue(v);
            })
            .AddTo(this);
    }
}
