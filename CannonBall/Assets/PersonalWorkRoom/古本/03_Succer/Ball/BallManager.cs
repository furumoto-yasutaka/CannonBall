/*******************************************************************************
*
*	タイトル：	ボールを管理するシングルトンスクリプト
*	ファイル：	BallManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : SingletonMonoBehaviour<BallManager>
{
    /// <summary> ボールのスポーン数について </summary>
    [System.Serializable]
    public class BallSpawnInfo
    {
        public int ChangeSecond = 0;
        public int BallSpawnMax = 0;
    }

    /// <summary> レアボールのスポーン確率について </summary>
    [System.Serializable]
    public class RareBallSpownRate
    {
        public int ChangeSecond = 0;
        public float RareBallSpawnRate = 0.0f;
    }

    /// <summary> 各ボールの情報 </summary>
    [System.Serializable]
    public class BallInfo
    {
        public GameObject Ball = null;
        public float SpownInterval = 0.0f;
    }

    /// <summary> 通常ボールのプレハブ </summary>
    [SerializeField, CustomLabel("通常ボールのプレハブ")]
    private GameObject m_ballPrefab;

    /// <summary> 金ボールのプレハブ </summary>
    [SerializeField, CustomLabel("金ボールのプレハブ")]
    private GameObject m_rareballPrefab;

    /// <summary> ボールの出現位置 </summary>
    [SerializeField, CustomLabel("ボールの出現位置")]
    private Vector3 m_ballSpownPos;

    /// <summary> ボールの出現位置差異 </summary>
    [SerializeField, CustomLabel("ボールの出現位置差異")]
    private Vector2 m_ballSpownPosOffset;

    [Header("ボールの同時出現上限に関する情報")]
    /// <summary> ボールの同時出現上限に関する情報 </summary>
    [SerializeField]
    private List<BallSpawnInfo> m_ballSpawnInfoList;

    [Header("金ボールの出現率に関する情報")]
    /// <summary> 金ボールの出現率に関する情報 </summary>
    [SerializeField]
    private List<RareBallSpownRate> m_rareBallSpownRateList;

    [Header("金ボールが出現しなかったら次確定で出現させる基準の秒数")]
    /// <summary> 金ボールが出現しなかったら次確定で出現させる基準の秒数 </summary>
    [SerializeField]
    private List<int> m_rareBallCompensatingTimeList;

    /// <summary> 最大何連続まで金ボールの出現を許すか </summary>
    [SerializeField, CustomLabel("最大何連続まで金ボールの出現を許すか")]
    private int m_rareBallConsecutivsMax;

    /// <summary> 
    /// ボールが足りなくなり、追加されるまでのクールタイム
    /// ※ボールの同時出現上限が多くなってその空きを追加する場合はクールタイム無し
    /// </summary>
    [SerializeField, CustomLabel("ボール追加までのクールタイム")]
    private int m_ballSpownCooltime;

    /// <summary> フィールドに存在しているボールの情報 </summary>
    [SerializeField]
    private List<BallInfo> m_balls = new List<BallInfo>();

    /// <summary> ボールの同時出現上限 </summary>
    [SerializeField, CustomLabelReadOnly("ボールの同時出現上限")]
    private int m_ballSpawnMax = 1;

    /// <summary> 金ボール出現確率 </summary>
    [SerializeField, CustomLabelReadOnly("金ボール出現確率")]
    private float m_rareBallSpawnRate = 0.5f;

    /// <summary> 指定時間内に金ボールが出現したか </summary>
    [SerializeField, CustomLabelReadOnly("指定時間内に金ボールが出現したか")]
    private bool m_isRareBallSpawn = false;

    /// <summary> 次のボールは金ボール確定か </summary>
    [SerializeField, CustomLabelReadOnly("次のボールは金ボール確定か")]
    private bool m_isNextSpownRareBall = false;

    /// <summary> 金ボールの連続出現数 </summary>
    [SerializeField, CustomLabelReadOnly("金ボールの連続出現数")]
    private int m_rareBallConsecutiveCount = 0;


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        // インスペクターの情報を秒数の降順に並び替え
        m_ballSpawnInfoList.Sort((a, b) => b.ChangeSecond - a.ChangeSecond);
        m_rareBallSpownRateList.Sort((a, b) => b.ChangeSecond - a.ChangeSecond);
        m_rareBallCompensatingTimeList.Sort();

        CheckRareBallSpownRate();
        CheckRareBallCompensatingTime();
        CheckBallSpawnInfo();
    }

    void Update()
    {
        CheckRareBallSpownRate();
        CheckRareBallCompensatingTime();
        CheckBallSpawnInfo();

        // クールタイムを確認し、ボールを追加する
        CheckBallSpownInterval();
    }

    /// <summary>
    /// 金のボールの出現率更新
    /// </summary>
    private void CheckRareBallSpownRate()
    {
        if (m_rareBallSpownRateList.Count > 0)
        {
            // 指定時間経過していたら更新
            if (m_rareBallSpownRateList[0].ChangeSecond >= Timer.Instance.m_TimeCounter)
            {
                m_rareBallSpawnRate = m_rareBallSpownRateList[0].RareBallSpawnRate;
                m_rareBallSpownRateList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// 金ボール確定出現について更新
    /// </summary>
    private void CheckRareBallCompensatingTime()
    {
        if (m_rareBallCompensatingTimeList.Count > 0)
        {
            // 指定時間経過していたら更新
            if (m_rareBallCompensatingTimeList[0] >= Timer.Instance.m_TimeCounter)
            {
                if (!m_isRareBallSpawn)
                {
                    m_isNextSpownRareBall = true;
                }
                m_isRareBallSpawn = false;
                m_rareBallCompensatingTimeList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// ボールの同時出現数更新
    /// </summary>
    private void CheckBallSpawnInfo()
    {
        if (m_ballSpawnInfoList.Count > 0)
        {
            // 指定時間経過していたら更新
            if (m_ballSpawnInfoList[0].ChangeSecond >= Timer.Instance.m_TimeCounter)
            {
                m_ballSpawnMax = m_ballSpawnInfoList[0].BallSpawnMax;
                m_ballSpawnInfoList.RemoveAt(0);
                AddBallInfo(m_ballSpawnMax);
            }
        }
    }

    /// <summary>
    /// 最初のボールを生成する
    /// </summary>
    private void CheckBallSpownInterval()
    {
        foreach (BallInfo info in m_balls)
        {
            if (info.SpownInterval > 0.0f)
            {
                info.SpownInterval -= Time.deltaTime;
                if (info.SpownInterval <= 0.0f)
                {
                    info.SpownInterval = 0.0f;
                    info.Ball = CreateBall();
                }
            }
        }
    }

    /// <summary>
    /// ボールの同時出現数増加に伴いボールの枠を追加する
    /// </summary>
    private void AddBallInfo(int size)
    {
        while (m_balls.Count < size)
        {
            m_balls.Add(new BallInfo());
            m_balls[m_balls.Count - 1].Ball = CreateBall();
        }
    }

    /// <summary>
    /// ボールを生成
    /// </summary>
    private GameObject CreateBall()
    {
        GameObject ball;
        
        if (m_isNextSpownRareBall)
        {// 金ボール確定
            ball = Instantiate(m_rareballPrefab);
            m_isNextSpownRareBall = false;
        }
        else if (m_rareBallConsecutiveCount >= m_rareBallConsecutivsMax)
        {// 金ボールの連続出現上限に達している
            ball = Instantiate(m_ballPrefab);
            m_rareBallConsecutiveCount = 0;
        }
        else
        {// 乱数で決定
            float rand = Random.Range(0.0f, 1.0f);
            if (rand >= m_rareBallSpawnRate)
            {// 通常ボール生成
                ball = Instantiate(m_ballPrefab);
                m_rareBallConsecutiveCount = 0;
            }
            else
            {// 金ボール生成
                ball = Instantiate(m_rareballPrefab);
                m_isRareBallSpawn = true;
                m_rareBallConsecutiveCount++;
            }
        }
        ball.transform.parent = null;

        Vector2 offset = Vector2.zero;
        offset.x = m_ballSpownPosOffset.x * Random.Range(-1.0f, 1.0f);
        offset.y = m_ballSpownPosOffset.y * Random.Range(-1.0f, 1.0f);
        ball.transform.position = m_ballSpownPos + (Vector3)offset;

        return ball;
    }

    /// <summary>
    /// ボールを削除
    /// </summary>
    public void DestroyBall(GameObject ball)
    {
        for (int i = 0; i < m_balls.Count; i++)
        {
            if (m_balls[i].Ball == ball)
            {
                m_balls[i].Ball = null;
                m_balls[i].SpownInterval = m_ballSpownCooltime;
                if (m_balls.Count > m_ballSpawnMax)
                {
                    m_balls.RemoveAt(i);
                }
                Destroy(ball);

                return;
            }
        }
    }
}
