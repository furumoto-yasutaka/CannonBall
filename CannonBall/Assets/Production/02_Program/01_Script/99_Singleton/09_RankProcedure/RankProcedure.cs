using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankProcedure : SingletonMonoBehaviour<RankProcedure>
{
    /// <summary> プレイヤーポイントコンポーネント </summary>
    private PlayerPoint_CannonFight[] m_playerPoints = new PlayerPoint_CannonFight[4];


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }

    private void Update()
    {
        int[] ranks = new int[4]
        {
            1,
            1,
            1,
            1,
        };

        // 順位付け
        for (int i = 0; i < ranks.Length; i++)
        {
            for (int j = i + 1; j < ranks.Length; j++)
            {
                if (m_playerPoints[i].m_Point.Value < m_playerPoints[j].m_Point.Value)
                {
                    ranks[i]++;
                }
                else if (m_playerPoints[i].m_Point.Value > m_playerPoints[j].m_Point.Value)
                {
                    ranks[j]++;
                }
            }

            // 順位を反映
            m_playerPoints[i].SetRank(ranks[i]);
        }
    }

    public void InitPlayer(PlayerPoint_CannonFight player, int id)
    {
        m_playerPoints[id] = player;
    }
}
