using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : SingletonMonoBehaviour<RespawnManager>
{
    // リスポーン待ちのプレイヤーの情報
    [System.Serializable]
    public class RespownInfo
    {
        public Transform m_Player;
        public bool m_IsRespawnInterval;
        public int m_FrameCount;

        public RespownInfo(Transform player, int interval)
        {
            m_Player = player;
            m_IsRespawnInterval = true;
            m_FrameCount = interval;
        }
    }

    /// <summary> 死亡からリスポーンまでの時間 </summary>
    [SerializeField, CustomLabel("死亡からリスポーンまでの時間(F)")]
    protected int m_respownInterval = 300;

    /// <summary> 死亡からリスポーンまでの時間 </summary>
    [SerializeField, CustomLabel("リスポーンボックス出現から強制復活までの時間(秒)")]
    protected float m_revivalInterval = 4.0f;

    /// <summary> トラックのプレハブ </summary>
    [SerializeField, CustomLabel("トラックのプレハブ")]
    protected GameObject m_truckPrefab;

    /// <summary> 一番最初のトラックのプレハブ </summary>
    [SerializeField, CustomLabel("一番最初のトラックのプレハブ")]
    protected GameObject m_firstTruckPrefab;

    /// <summary> リスポーン待機リスト </summary>
    [SerializeField, CustomLabelReadOnly("リスポーン待機リスト")]
    protected List<RespownInfo> m_respownList = new List<RespownInfo>();

    /// <summary> 初期スポーンカーが来るまでの遅延時間 </summary>
    [SerializeField, CustomLabel("初期スポーンカーが来るまでの遅延時間")]
    protected float m_firstRespawnDelay = 2.0f;

    private bool m_IsSetRespawnAll = false;

    private Transform[] m_IsSetRespawnPlayers;

    protected float m_firstRespawnDelayCount = 0.0f;


    public float m_RevivalInterval { get { return m_revivalInterval; } }

    public int m_RespownListCount { get { return m_respownList.Count; } }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }

    protected void FixedUpdate()
    {
        for (int i = 0; i < m_respownList.Count; i++)
        {
            // リスポーンのインターバル待ち中か
            if (m_respownList[i].m_IsRespawnInterval)
            {
                m_respownList[i].m_FrameCount--;
                if (m_respownList[i].m_FrameCount <= 0.0f)
                {
                    // リスポーン開始
                    StartRespawn(m_respownList[i].m_Player);
                    m_respownList[i].m_IsRespawnInterval = false;
                }
            }
        }

        if (m_IsSetRespawnAll)
        {
            // 開始の演出があるため遅延させる
            m_firstRespawnDelayCount -= Time.deltaTime;

            if (m_firstRespawnDelayCount <= 0.0f)
            {
                m_IsSetRespawnAll = false;

                GameObject truck = Instantiate(m_firstTruckPrefab);
                truck.GetComponent<FirstRespawnTruck>().SetPlayers(m_IsSetRespawnPlayers);
            }
        }
    }

    protected virtual void StartRespawn(Transform player)
    {
        GameObject truck = Instantiate(m_truckPrefab);
        truck.GetComponent<RespawnTruck>().SetPlayer(player);

        AudioManager.Instance.PlaySe(
            "リスポーン_クラクション",
            false);
    }

    public virtual void StartRespawnAll(Transform[] players)
    {
        m_IsSetRespawnAll = true;
        m_IsSetRespawnPlayers = players;
        m_firstRespawnDelayCount = m_firstRespawnDelay;
    }

    public virtual void EndRespawn(Transform player)
    {
        for (int i = 0; i < m_respownList.Count; i++)
        {
            if (m_respownList[i].m_Player == player)
            {
                m_respownList.RemoveAt(i);
                return;
            }
        }
    }

    public virtual void AddRespawnPlayer(Transform player)
    {
        m_respownList.Add(new RespownInfo(player, m_respownInterval));
    }

    public virtual void SetRespawnAnimation(Transform player)
    {
        Animator anim = player.GetChild(0).GetComponent<Animator>();
        anim.SetBool("IsRespawn", true);
        anim.SetInteger("RespawnSwitch", player.GetComponent<PlayerId>().m_Id);
    }

    public void SetFirstRespawnAnimation(Transform player)
    {
        Animator anim = player.GetChild(0).GetComponent<Animator>();
        anim.SetBool("IsFirstRespawn", true);
        anim.SetInteger("RespawnSwitch", player.GetComponent<PlayerId>().m_Id);
    }
}
