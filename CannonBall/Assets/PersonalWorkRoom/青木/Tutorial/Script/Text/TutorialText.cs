using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField]
    int[] m_eventTextLine;

    TextFeed m_textFeed;


    private void Start()
    {
        m_textFeed = GetComponent<TextFeed>();
    }



    private void Update()
    {
        if (Event())
        {
            m_textFeed.NextTextLine();
        }
        else if (m_textFeed.GetIsEndFeed)
        {
            m_textFeed.NextTextLine();
        }
    }



    private bool Event()
    {
        for (int i = 0; i < m_eventTextLine.Length; i++) 
        {
            // 進行中のテキストが終わった ＆ 次のテキスト番号が何かしらのイベントがあるのなら
            if (m_textFeed.GetIsEndFeed && m_textFeed.GetTextLine + 1 == m_eventTextLine[i])
            {
                return true;
            }
        }
        return false;
    }

}
