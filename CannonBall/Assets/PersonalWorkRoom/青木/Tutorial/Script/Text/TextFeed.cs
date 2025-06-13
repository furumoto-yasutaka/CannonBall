/*******************************************************************************
*
*	タイトル：	文字送り
*	ファイル：	TextFeed.cs
*	作成者：	青木 大夢
*	制作日：    2023/10/09
*
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;



public class TextFeed : MonoBehaviour
{

    [SerializeField]
    private TextAsset m_textAsset;

    [SerializeField, CustomLabel("文字送りのスピード(単位：f)")]
    private int m_textFeedFrameSpeed = 1;

    private int m_textFeedFrameCount = 0;


    [SerializeField, CustomLabel("行間スピード(単位：f)")]
    private int m_textWait = 5;

    [SerializeField, CustomLabel("3点リーダーを印字するか")]
    private bool m_3MarkOutput = true;

    private int m_textWaitCount = 0;

    // TextAssetから改行毎に区切って格納
    private string[] m_text;

    private int m_textLine = 0;

    public int GetTextLine { get { return m_textLine; } }

    private TextMeshProUGUI m_textMeshPro;

    private bool m_isFeed = true;

    private bool m_isEventChar = false;

    public bool IsEventChar { get { return m_isEventChar; } set { m_isEventChar = value; } }

    public bool GetIsFeed { get { return m_isFeed; } }

    private bool m_isEndFeed = false;
    public bool GetIsEndFeed { get { return m_isEndFeed; } }

    private int m_charCount = 0;

    // 句読点の待つ時間（単位：秒）
    private readonly float m_PunctuationMark = 0.1f;

    // 三点リーダーの待つ時間（単位：秒）
    private readonly float m_3Mark = 0.45f;

    // 句読点、句点などの待つ時間
    private float m_waitTime;


    // 何かしらのイベントが起きて、テキストが止まる処理
    private bool m_isStop = false;

    public bool m_IsStop { get { return m_isStop; } set { m_isStop = value; } }

    // テキストを流していく最中に関数を実行する用の仕組み
    [SerializeField]
    private UnityEvent[] m_textActions;
    // テキストを流していく最中に実行する関数の添え字
    int m_textActionIndex = 0;



    private void Start()
    {

        string text = m_textAsset.text;
        m_text = text.Split(char.Parse("\n"));


        m_textMeshPro = GetComponent<TextMeshProUGUI>();

        InitializeText();

    }


    private void Update()
    {
        if (m_isStop)
        {
            return;
        }

        if (m_isFeed && !m_isEndFeed)
        {


            if (m_waitTime >= 0.0f)
            {
                m_waitTime -= Time.deltaTime;
            }
            else if (ContainsMark('、'))
            {
                WaitFeed(m_PunctuationMark, true);
            }
            else if (ContainsMark('…'))
            {
                WaitFeed(m_3Mark, m_3MarkOutput);
            }
            else
            {

                m_textFeedFrameCount++;
                if (m_textFeedFrameCount >= m_textFeedFrameSpeed)
                {
                    if (m_text[m_textLine][m_charCount] == '#')
                    {
                        m_isStop = true;
                        m_isEventChar = true;
                    }
                    else if (ContainsMark('@'))
                    {
                        // 登録されている関数を実行
                        m_textActions[m_textActionIndex++].Invoke();
                    }
                    else
                    {
                        m_textMeshPro.text += m_text[m_textLine][m_charCount];

                        AudioManager.Instance.PlaySe("チュートリアル_会話", false);

                    }


                    m_charCount++;

                    m_textFeedFrameCount = 0;


                    // m_manualText[m_textLine]の最後の文字まで表示したら
                    if (m_charCount >= m_text[m_textLine].Length)
                    {
                        //// イベントが詰まっていたら、文末で止まる
                        //if (m_isEventChar)
                        //{
                        //    m_isStop = true;
                        //}
                        m_isFeed = false;

                        m_isEndFeed = true;
                    }
                }
            }
        }
        else if (m_isEndFeed)
        {
            // 行間を待つ
            if (m_textWaitCount > m_textWait)
            {
                // 次の行を設定する
                NextTextLine();

                m_textWaitCount = 0;
            }
            else
            {
                m_textWaitCount++;
            }
        }
    }

    // 次に流す文字が引数の文字かどうかを判別
    private bool ContainsMark(char _char)
    {
        return m_text[m_textLine][m_charCount] == _char && m_waitTime < 0.0f;
    }

    private void WaitFeed(float _waitTime, bool _isOutput)
    {
        m_waitTime = m_3Mark;

        if (_isOutput)
        {
            m_textMeshPro.text += m_text[m_textLine][m_charCount];
        }

        m_charCount++;
    }


    public bool NextTextLine()
    {
        if (m_text.Length - 1 > m_textLine)
        {
            m_textLine++;

            // テキストの初期化
            InitializeText();

            return true;
        }

        return false;
    }


    public void EndEvent()
    {
        m_isEventChar = false;
        m_isStop = false;
    }

    private void InitializeText()
    {
        // 番号リセット
        m_charCount = 0;

        // 現在のテキストをクリア
        m_textMeshPro.text = "";

        // テキストの縦幅を変える
        m_textMeshPro.rectTransform.sizeDelta = new Vector2(m_textMeshPro.fontSize * m_text[m_textLine].Length, m_textMeshPro.fontSize);

        // テキスト送りのスイッチをONにする
        m_isFeed = true;

        // 文を止めるフラグをOFF
        m_isStop = false;

        // 行末フラグをOFF
        m_isEndFeed = false;
    }
}
