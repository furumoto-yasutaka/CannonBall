/*******************************************************************************
*
*	タイトル：	タイマーシングルトンスクリプト
*	ファイル：	Timer.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : SingletonMonoBehaviour<Timer>
{
    /// <summary> 数値の画像素材 </summary>
    [SerializeField, CustomLabel("数値の画像素材")]
    protected Sprite[] m_numberSprites;

    /// <summary> 試合時間(秒) </summary>
    [SerializeField, CustomLabel("試合時間(秒)")]
    protected int m_timelimit = 180;

    /// <summary> 分表示の親オブジェクト </summary>
    [SerializeField, CustomLabel("分表示の親オブジェクト")]
    protected Transform[] m_minutesParent;

    /// <summary> 分表示の親オブジェクト </summary>
    [SerializeField, CustomLabel("秒表示の親オブジェクト")]
    protected Transform[] m_secondsParent;

    /// <summary> タイマーを動かさない </summary>
    [SerializeField, CustomLabel("タイマーを動かさない")]
    public bool m_isStopTimer = true;

    /// <summary> タイマーの残り時間 </summary>
    protected float m_timeCounter = 9999.0f;

    protected bool m_isTimerEnd = false;


    public float m_TimeCounter { get { return m_timeCounter; } }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_timeCounter = m_timelimit;

        // タイマーの値を初期化
        m_timeCounter += Time.deltaTime;
        TimeUpdate();
    }

    protected virtual void Update()
    {
        if (m_isStopTimer)
        {
            if (ReadyGoAnimationCallback.CheckInstance())
            {
                if (ReadyGoAnimationCallback.Instance.m_IsFinish)
                {
                    m_isStopTimer = false;
                }
            }
        }
        else
        {
            TimeUpdate();
        }
    }

    private void TimeUpdate()
    {
        if (m_timeCounter > 0.0f)
        {
            // 経過秒数を減算
            m_timeCounter -= Time.deltaTime;

            if (m_timeCounter <= 0.0f)
            {
                m_isTimerEnd = true;
                m_timeCounter = 0.0f;
                TimerEndCallback();
            }

            int count = (int)m_timeCounter;
            // 小数点以下の端数が存在する場合1秒加算して表示する
            if (m_timeCounter - count > 0.0f)
            {
                count++;
            }
            int minute = count / 60;
            int second = count % 60;

            // 分の画像設定
            SetNumberSprite(minute, m_minutesParent);

            // 秒の画像設定
            SetNumberSprite(second, m_secondsParent);
        }
    }

    /// <summary> 得点スプライト設定 </summary>
    /// <param name="value"> 設定する値 </param>
    /// <param name="parent"> 数値表示の親オブジェクト </param>
    protected virtual void SetNumberSprite(int value, Transform[] parent)
    {
        for (int i = 0; i < parent.Length; i++)
        {
            int temp = value;
            int j = 0;
            do
            {
                int d = temp % 10;
                parent[i].GetChild(j).GetComponent<Image>().sprite = m_numberSprites[d];
                temp /= 10;
                j++;
            }
            while (j < parent[i].childCount && temp != 0);

            for (; j < parent[i].childCount; j++)
            {
                parent[i].GetChild(j).GetComponent<Image>().sprite = m_numberSprites[0];
            }
        }
    }

    protected virtual void TimerEndCallback()
    {

    }
}
