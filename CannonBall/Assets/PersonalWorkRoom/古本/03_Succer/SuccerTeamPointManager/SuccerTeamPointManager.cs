/*******************************************************************************
*
*	タイトル：	サッカーの得点管理シングルトンスクリプト
*	ファイル：	PointManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SuccerTeamPointManager : SingletonMonoBehaviour<SuccerTeamPointManager>
{
    /// <summary> 赤の得点 </summary>
    [SerializeField, CustomLabelReadOnly("赤の得点")]
    private ReactiveProperty<int> m_redPoint = new ReactiveProperty<int>(0);

    /// <summary> 赤の得点 </summary>
    [SerializeField, CustomLabelReadOnly("青の得点")]
    private ReactiveProperty<int> m_bluePoint = new ReactiveProperty<int>(0);

    /// <summary> ボールが入ったときのポイント </summary>
    [SerializeField, CustomLabel("ボールが入ったときのポイント")]
    private int m_ballInPoint = 4;

    /// <summary> ボールが入ったときのポイント </summary>
    [SerializeField, CustomLabel("金ボールが入ったときのポイント")]
    private int m_rareballInPoint = 4;

    /// <summary> プレイヤーが入ったときのポイント </summary>
    [SerializeField, CustomLabel("プレイヤーが入ったときのポイント")]
    private int m_playerInPoint = 1;

    /// <summary> 試合終了のボーダーのポイント </summary>
    [SerializeField, CustomLabel("試合終了のボーダーのポイント")]
    private int m_finishBorderPoint = 30;

    /// <summary> 試合が終了したかどうか </summary>
    private bool m_isFinish = false;


    public IReadOnlyReactiveProperty<int> m_RedPoint => m_redPoint;

    public IReadOnlyReactiveProperty<int> m_BluePoint => m_bluePoint;

    public bool m_IsFinish => m_isFinish;


    /// <summary> 赤チームへ通常ボールのゴールポイントを加算 </summary>
    public void BallGoalIn_Red()
    {
        if (m_isFinish) { return; }
        m_redPoint.Value += m_ballInPoint;
        CheckFinish(m_redPoint.Value);
    }

    /// <summary> 青チームへ通常ボールのゴールポイントを加算 </summary>
    public void BallGoalIn_Blue()
    {
        if (m_isFinish) { return; }
        m_bluePoint.Value += m_ballInPoint;
        CheckFinish(m_bluePoint.Value);
    }

    /// <summary> 赤チームへ金ボールのゴールポイントを加算 </summary>
    public void RareBallGoalIn_Red()
    {
        if (m_isFinish) { return; }
        m_redPoint.Value += m_rareballInPoint;
        CheckFinish(m_redPoint.Value);
    }

    /// <summary> 青チームへ金ボールのゴールポイントを加算 </summary>
    public void RareBallGoalIn_Blue()
    {
        if (m_isFinish) { return; }
        m_bluePoint.Value += m_rareballInPoint;
        CheckFinish(m_bluePoint.Value);
    }

    /// <summary> 赤チームへ相手プレイヤー自滅のポイントを加算 </summary>
    public void PlayerGoalIn_Red()
    {
        if (m_isFinish) { return; }
        m_redPoint.Value += m_playerInPoint;
        CheckFinish(m_redPoint.Value);
    }

    /// <summary> 青チームへ相手プレイヤー自滅のポイントを加算 </summary>
    public void PlayerGoalIn_Blue()
    {
        if (m_isFinish) { return; }
        m_bluePoint.Value += m_playerInPoint;
        CheckFinish(m_bluePoint.Value);
    }

    private void CheckFinish(int point)
    {
        if (point >= m_finishBorderPoint)
        {
            m_isFinish = true;
        }
    }
}
