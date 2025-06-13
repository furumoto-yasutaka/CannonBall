using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerSpMovePoint_Presenter : MonoBehaviour
{
    [Header("View")]

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 1P")]
    private PlayerSpMovePoint_View m_view_1P;

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 2P")]
    private PlayerSpMovePoint_View m_view_2P;

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 3P")]
    private PlayerSpMovePoint_View m_view_3P;

    /// <summary> ポイント表示制御コンポーネント </summary>
    [SerializeField, CustomLabel("ポイント表示 4P")]
    private PlayerSpMovePoint_View m_view_4P;


    [Header("Model")]

    /// <summary> プレイヤーの親オブジェクト </summary>
    [SerializeField, CustomLabel("プレイヤーの親オブジェクト")]
    private Transform m_playerParent;

    /// <summary> 必殺技コンポーネント1P </summary>
    private PlayerSpMove m_point_1P;
    /// <summary> 必殺技コンポーネント2P </summary>
    private PlayerSpMove m_point_2P;
    /// <summary> 必殺技コンポーネント3P </summary>
    private PlayerSpMove m_point_3P;
    /// <summary> 必殺技コンポーネント4P </summary>
    private PlayerSpMove m_point_4P;


    private void Awake()
    {
        m_point_1P = m_playerParent.GetChild(0).GetComponent<PlayerSpMove>();
        m_point_1P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_1P.SetSliderValue(v);
            })
            .AddTo(this);
        m_point_2P = m_playerParent.GetChild(1).GetComponent<PlayerSpMove>();
        m_point_2P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_2P.SetSliderValue(v);
            })
            .AddTo(this);
        m_point_3P = m_playerParent.GetChild(2).GetComponent<PlayerSpMove>();
        m_point_3P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_3P.SetSliderValue(v);
            })
            .AddTo(this);
        m_point_4P = m_playerParent.GetChild(3).GetComponent<PlayerSpMove>();
        m_point_4P.m_SpMovePointRate.Subscribe(v =>
            {
                m_view_4P.SetSliderValue(v);
            })
            .AddTo(this);
    }
}
