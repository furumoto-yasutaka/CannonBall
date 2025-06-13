using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data.Common;

public class ResultPoints : MonoBehaviour
{
    TextMeshProUGUI[] m_tmps;

    private void OnEnable()
    {
        m_tmps = new TextMeshProUGUI[transform.childCount];

        for (int i = 0; i < m_tmps.Length; i++)
        {
            m_tmps[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        int tempIndex = 0;
        int[] len = TemporaryData.GetRankCount();   // èáà âΩéÌóﬁÅH
        for (int i = 0; i < len.Length; i++)
        {
            TemporaryData.Rank[] rank = TemporaryData.GetRank(i + 1);
            for (int j = 0; j < rank.Length; j++)
            {
                float point = rank[j].m_Parameter;
                point *= 10.0f;
                point = Mathf.Round(point);
                point *= 0.1f;

                m_tmps[tempIndex].text = point.ToString();
                m_tmps[tempIndex].text += "pt";

                tempIndex++;
            }
        }
    }
}
