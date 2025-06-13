/*******************************************************************************
*
*	タイトル：	死亡回数表示制御仲介スクリプト
*	ファイル：	DeathCountUi_Presenter.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/10
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DeathCountUi_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> 死亡数表示 1P </summary>
    [SerializeField, CustomLabel("死亡数表示 1P")]
    private DeachCountUi_View m_view_1P;

    /// <summary> 死亡数表示 2P </summary>
    [SerializeField, CustomLabel("死亡数表示 2P")]
    private DeachCountUi_View m_view_2P;

    /// <summary> 死亡数表示 3P </summary>
    [SerializeField, CustomLabel("死亡数表示 3P")]
    private DeachCountUi_View m_view_3P;

    /// <summary> 死亡数表示 4P </summary>
    [SerializeField, CustomLabel("死亡数表示 4P")]
    private DeachCountUi_View m_view_4P;


    [Header("Model")]

    /// <summary> 死亡数制御 1P </summary>
    [SerializeField, CustomLabel("死亡数制御 1P")]
    private PlayerDeathCount m_deathCount_1P;

    /// <summary> 死亡数制御 2P </summary>
    [SerializeField, CustomLabel("死亡数制御 2P")]
    private PlayerDeathCount m_deathCount_2P;

    /// <summary> 死亡数制御 3P </summary>
    [SerializeField, CustomLabel("死亡数制御 3P")]
    private PlayerDeathCount m_deathCount_3P;

    /// <summary> 死亡数制御 4P </summary>
    [SerializeField, CustomLabel("死亡数制御 4P")]
    private PlayerDeathCount m_deathCount_4P;


    private void Start()
    {
        m_view_1P.Init();
        m_view_2P.Init();
        m_view_3P.Init();
        m_view_4P.Init();

        m_deathCount_1P.m_DeathCount.Subscribe(v =>
            {
                m_view_1P.SetValue(v);
            })
            .AddTo(this);
        m_deathCount_2P.m_DeathCount.Subscribe(v =>
            {
                m_view_2P.SetValue(v);
            })
            .AddTo(this);
        m_deathCount_3P.m_DeathCount.Subscribe(v =>
            {
                m_view_3P.SetValue(v);
            })
            .AddTo(this);
        m_deathCount_4P.m_DeathCount.Subscribe(v =>
            {
                m_view_4P.SetValue(v);
            })
            .AddTo(this);
    }
}
