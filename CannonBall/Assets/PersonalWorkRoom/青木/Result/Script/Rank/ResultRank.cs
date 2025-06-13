using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ResultRank : MonoBehaviour
{
    // 長いので省略変数を用意
    readonly int DATA_LENGTH = TemporaryData.m_Rank.Length;

    private void Awake()
    {

        // 初期化
        for (int i = 0; i < DATA_LENGTH; i++)
        {
            // 初期値０
            TemporaryData.m_Rank[i].m_Ranking = 0;
        }


        // ソートされた配列
        int[] parameters = new int[4];
        for (int i = 0; i < DATA_LENGTH; i++)
        {
            // パラメーターを10倍にして、小数点第一位まで順位の考慮に入れる。第二位以下は切り捨て
            parameters[i] = Mathf.FloorToInt(TemporaryData.m_Rank[i].m_Parameter * 10.0f);

        }


        // 順位
        int ranking = 1;

        // 配列を昇順ソート
        Array.Sort(parameters);

        // デンジャランの場合は、順位が反転
        if (TemporaryData.m_PreGameMode != SceneNameEnum.DangerRun)
        {
            // 配列の順序を反転させる
            Array.Reverse(parameters);
        }

        int preParam = parameters[0];


        // 順位付け実行部分
        for (int i = 0; i < DATA_LENGTH; i++)
        {
            for (int j = 0; j < DATA_LENGTH; j++)
            {
                if (parameters[i] == Mathf.FloorToInt(TemporaryData.m_Rank[j].m_Parameter * 10.0f) && TemporaryData.m_Rank[j].m_Ranking == 0)
                {
                    TemporaryData.m_Rank[j].m_Ranking = (preParam != parameters[i]) ? ++ranking : ranking;
                    break;
                }
            }

            preParam = parameters[i];
        }
    }


}
