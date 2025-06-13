/*******************************************************************************
*
*	タイトル：	プレイヤーのポイント制御スクリプト
*	ファイル：	PlayerPoint_CannonFight.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerPoint_CannonFight : MonoBehaviour, ITemporaryDataGetter
{
    /// <summary> 順位 </summary>
    private int m_rank = 1;

    /// <summary> 得点 </summary>
    private ReactiveProperty<int> m_point = new ReactiveProperty<int>(0);

    /// <summary> 前回の得点増減で増えたかどうか </summary>
    private bool m_isAdd = true;

    /// <summary> 蹴りによるマークを受けているか </summary>
    private bool m_isKickMark = true;

    /// <summary> マークされているプレイヤー </summary>
    private PlayerPoint_CannonFight m_markPlayer = null;

    /// <summary> マークの残り時間 </summary>
    private float m_markTimeCount = 0.0f;

    /// <summary> キックされた時のマーク時間 </summary>
    private static float m_fromKickMarkTime = 6.0f;

    /// <summary> 頭突かれた時のマーク時間 </summary>
    private static float m_fromContactMarkTime = 3.0f;

    /// <summary> 頭突いたとするベクトルの強さ </summary>
    private static float m_contactMarkThreshold = 1.0f;


    public IReadOnlyReactiveProperty<int> m_Point => m_point;

    public PlayerPoint_CannonFight m_MarkPlayer { get { return m_markPlayer; } }

    public bool m_IsAdd { get { return m_isAdd; } }

    public bool m_IsKickMark { get { return m_isKickMark; } }

    public int m_Rank { get { return m_rank; } }


    private void Update()
    {
        // マーク時間を更新
        if (m_markPlayer != null)
        {
            if (m_markTimeCount <= 0.0f)
            {
                m_markTimeCount = 0.0f;
                m_markPlayer = null;
                m_isKickMark = false;
            }
            else
            {
                m_markTimeCount -= Time.deltaTime;
            }
        }
    }

    /// <summary> キルされた際のポイント清算 </summary>
    public void KilledDividePoint()
    {
        SubPoint();

        if (m_markPlayer != null)
        {
            m_markPlayer.AddPoint();
        }
    }

    /// <summary> ポイント加算 </summary>
    public void AddPoint()
    {
        m_point.Value++;
        m_isAdd = true;
    }

    /// <summary> ポイント減少 </summary>
    public void SubPoint()
    {
        m_point.Value--;
        m_isAdd = false;
    }

    /// <summary> 蹴りによるマークを行う </summary>
    /// <param name="target"> プレイヤーのコリジョン </param>
    public void RequestKickMark(PlayerPoint_CannonFight target)
    {
        m_markPlayer = target;
        m_markTimeCount = m_fromKickMarkTime;
        m_isKickMark = true;
    }

    /// <summary> 頭突きによるマークを行う </summary>
    /// <param name="target"> 対象のプレイヤー </param>
    /// <param name="vel"> 頭突きのベクトル </param>
    public void RequestContactMark(PlayerPoint_CannonFight target, Vector2 vel)
    {
        // 頭突きの強さが一定以上かつ
        // 現在残っているマーク時間が頭突きで付与されるマーク時間より小さいか
        if (vel.sqrMagnitude >= m_contactMarkThreshold * m_contactMarkThreshold &&
            m_markTimeCount <= m_fromContactMarkTime)
        {
            if (target.m_IsKickMark)
            {
                m_markPlayer = target.m_MarkPlayer;
                m_markTimeCount = m_fromContactMarkTime;
            }
            else
            {
                m_markPlayer = target;
                m_markTimeCount = m_fromContactMarkTime;
            }

            m_isKickMark = false;
        }
    }

    /// <summary> 順位設定 </summary>
    /// <param name="rank"> 順位 </param>
    public void SetRank(int rank)
    {
        m_rank = rank;
    }

    public float GetRankingParameter()
    {
        return m_point.Value;
    }
}
