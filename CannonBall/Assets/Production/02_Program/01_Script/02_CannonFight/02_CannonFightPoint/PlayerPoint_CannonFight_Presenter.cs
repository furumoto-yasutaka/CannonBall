/*******************************************************************************
*
*	タイトル：	プレイヤーのポイント表示用データ
*	ファイル：	PlayerPoint_CannonFight_Data.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerPoint_CannonFight_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 1P")]
    private PlayerPoint_CannonFight_View m_view_1P;

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 2P")]
    private PlayerPoint_CannonFight_View m_view_2P;

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 3P")]
    private PlayerPoint_CannonFight_View m_view_3P;

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 4P")]
    private PlayerPoint_CannonFight_View m_view_4P;


    [Header("Model")]

    /// <summary> プレイヤーの親オブジェクト </summary>
    [SerializeField, CustomLabel("プレイヤーの親オブジェクト")]
    private Transform m_playerParent;

    /// <summary> ポイント制御コンポーネント1P </summary>
    private PlayerPoint_CannonFight m_point_1P;
    /// <summary> ポイント制御コンポーネント2P </summary>
    private PlayerPoint_CannonFight m_point_2P;
    /// <summary> ポイント制御コンポーネント3P </summary>
    private PlayerPoint_CannonFight m_point_3P;
    /// <summary> ポイント制御コンポーネント4P </summary>
    private PlayerPoint_CannonFight m_point_4P;


    private void Awake()
    {
        m_view_1P.Init();
        m_view_2P.Init();
        m_view_3P.Init();
        m_view_4P.Init();

        m_point_1P = m_playerParent.GetChild(0).GetComponent<PlayerPoint_CannonFight>();
        m_point_1P.m_Point.Subscribe(v =>
            {
                m_view_1P.SetValue(v, m_point_1P.m_IsAdd);
            })
            .AddTo(this);
        m_point_2P = m_playerParent.GetChild(1).GetComponent<PlayerPoint_CannonFight>();
        m_point_2P.m_Point.Subscribe(v =>
            {
                m_view_2P.SetValue(v, m_point_2P.m_IsAdd);
            })
            .AddTo(this);
        m_point_3P = m_playerParent.GetChild(2).GetComponent<PlayerPoint_CannonFight>();
        m_point_3P.m_Point.Subscribe(v =>
            {
                m_view_3P.SetValue(v, m_point_3P.m_IsAdd);
            })
            .AddTo(this);
        m_point_4P = m_playerParent.GetChild(3).GetComponent<PlayerPoint_CannonFight>();
        m_point_4P.m_Point.Subscribe(v =>
            {
                m_view_4P.SetValue(v, m_point_4P.m_IsAdd);
            })
            .AddTo(this);
    }
}
