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
        public int m_Ranking;       // �����l�O
        public float m_Parameter;
    }

    //�@��ɔz�񏇏��̓����N���ɐ��񂵂Ă���
    static public Rank[] m_Rank = new Rank[4];

    // ���U���g�ɓ���O�̃V�[��
    static public SceneNameEnum m_PreGameMode = SceneNameEnum.CannonFight;

    /// <summary>
    /// �����œn���ꂽ���ʂ̃����N�^�z�񂪓n�����
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


    
    /// <summary> ���U���g�̏��ʂ������N���Ƃɔz��ɂ������̂��v�Z </summary>
    /// <returns> ���U���g�̏��ʂ������N���Ƃɔz��ɂ������� �ő咷���S</returns>
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
    /// �����Ŏw�肵�������N��������̂��H�@�����ʂ̏ꍇ��2�Ƃ����\�������
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
