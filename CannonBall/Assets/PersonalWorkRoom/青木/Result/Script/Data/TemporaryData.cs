using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;

public class TemporaryData
{
    [System.Serializable]
    public struct Rank
    {
        public int m_PlayerId;
        public int m_Ranking;       // 初期値０
        public float m_Parameter;
    }

    //　常に配列順序はランク順に整列しておく
    static public Rank[] m_Rank = new Rank[4];

    // リザルトに入る前のシーン
    static public SceneNameEnum m_PreGameMode = SceneNameEnum.CannonFight;

    /// <summary>
    /// 引数で渡された順位のランク型配列が渡される
    /// </summary>
    /// <param name="_rank"></param>
    /// <returns></returns>
    static public Rank[] GetRank(int _rank)
    {
        List<Rank> list = new List<Rank>();

        for (int i = 0; i < m_Rank.Length; i++)
        {
            if (m_Rank[i].m_Ranking == _rank)
            {
                list.Add(m_Rank[i]);
            }
        }
        return list.ToArray();
    }


    
    /// <summary> リザルトの順位をランクごとに配列にしたものを計算 </summary>
    /// <returns> リザルトの順位をランクごとに配列にしたもの 最大長さ４</returns>
    static public int[] GetRankCount()
    {
        List<int> list = new List<int>();

        for (int i = 0; i < m_Rank.Length; i++)
        {
            list.Add(GetSearch(i));
        }

        return list.ToArray();
    }

    /// <summary>
    /// 引数で指定したランクが何個あるのか？　同順位の場合は2とかが表示される
    /// </summary>
    /// <param name="_targetRank"></param>
    /// <returns></returns>
    static public int GetSearch(int _targetRank)
    {
        int result = 0;

        for (int i = 0; i < m_Rank.Length; i++)
        {
            if (_targetRank == m_Rank[i].m_Ranking)
            {
                result++;
            }
        }

        return result;
    }


}
