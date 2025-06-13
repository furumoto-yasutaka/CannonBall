using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
    // チュートリアルの段階の数字
    // 0：右の壁に当たる
    // 1：左の天井に当たる
    // 2：上の天井に当たる
    int m_tutorialLevel = 0;

    [SerializeField]
    TextFeed m_textFeed;

    [SerializeField, CustomLabel("テキストと見比べて、何行目にイベントが必要なのか")]
    int[] m_eventTextLine;

    bool m_isEventing = false;

    GameObject[] m_tutorialWalls;

    private void Start()
    {
        m_tutorialWalls = new GameObject[transform.childCount];
        for (int i = 0; i < m_tutorialWalls.Length; i++)
        {
            m_tutorialWalls[i] = transform.GetChild(i).gameObject;

            // 一番最初以外をOFFにする
            //if (i != 0)
            {
                m_tutorialWalls[i].gameObject.SetActive(false);
            }
        }


    }

    private void Update()
    {
        if (m_isEventing)
        {
            return;
        }


        if (Event())
        {
            Debug.Log("あ");

            //m_textFeed.NextTextLine();
            //StartCoroutine(NextText());
            if (m_textFeed.GetIsEndFeed)
            {
                m_textFeed.m_IsStop = true;
            }


        }

        if (m_textFeed.IsEventChar)
        {
            m_tutorialWalls[m_tutorialLevel].gameObject.SetActive(true);
        }
        ////else if (StopEvent())
        ////{

        ////    m_isEventing = true;

        ////}
        //else if (NoEvent())
        //{
        //    Debug.Log("ああ");
        //    m_textFeed.NextTextLine();
        //    //StartCoroutine(NextText());
        //}
    }


    IEnumerator NextText()
    {
        yield return new WaitForSeconds(0.5f);

        m_textFeed.NextTextLine();
    }


    private bool Event()
    {
        for (int i = 0; i < m_eventTextLine.Length; i++)
        {
            // 進行中のテキストが終わった ＆ 次のテキスト番号が何かしらのイベントがあるのなら
            if (m_textFeed.IsEventChar && m_textFeed.GetTextLine == m_eventTextLine[i])
            {
                return true;
            }
        }
        return false;
    }


    private bool StopEvent()
    {
        for (int i = 0; i < m_eventTextLine.Length; i++)
        {
            // 進行中のテキストが終わった ＆ 次のテキスト番号が何かしらのイベントがあるのなら
            if (m_textFeed.GetIsEndFeed && m_textFeed.GetTextLine == m_eventTextLine[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool NoEvent()
    {
        // なんかめちゃ処理は知っちゃう



        // テキストが終了していなかったら、そもそも次のテキストに送らない
        if (!m_textFeed.GetIsEndFeed)
        {
            return false;
        }

        for (int i = 0; i < m_eventTextLine.Length; i++)
        {
            // 次のテキスト番号が何かしらのイベントがない
            if (m_textFeed.GetTextLine + 1 == m_eventTextLine[i])
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// 全員が一段階チュートリアルが終わったら
    /// </summary>
    public void ChangeNextLevel()
    {
        m_tutorialWalls[m_tutorialLevel].gameObject.SetActive(false);

        m_tutorialLevel++;

        m_isEventing = false;

        m_textFeed.NextTextLine();

        m_textFeed.EndEvent();
    }



}
