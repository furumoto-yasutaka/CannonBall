using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : SingletonMonoBehaviour<RespawnManager>
{
    // ���X�|�[���҂��̃v���C���[�̏��
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

    /// <summary> ���S���烊�X�|�[���܂ł̎��� </summary>
    [SerializeField, CustomLabel("���S���烊�X�|�[���܂ł̎���(F)")]
    protected int m_respownInterval = 300;

    /// <summary> ���S���烊�X�|�[���܂ł̎��� </summary>
    [SerializeField, CustomLabel("���X�|�[���{�b�N�X�o�����狭�������܂ł̎���(�b)")]
    protected float m_revivalInterval = 4.0f;

    /// <summary> �g���b�N�̃v���n�u </summary>
    [SerializeField, CustomLabel("�g���b�N�̃v���n�u")]
    protected GameObject m_truckPrefab;

    /// <summary> ��ԍŏ��̃g���b�N�̃v���n�u </summary>
    [SerializeField, CustomLabel("��ԍŏ��̃g���b�N�̃v���n�u")]
    protected GameObject m_firstTruckPrefab;

    /// <summary> ���X�|�[���ҋ@���X�g </summary>
    [SerializeField, CustomLabelReadOnly("���X�|�[���ҋ@���X�g")]
    protected List<RespownInfo> m_respownList = new List<RespownInfo>();

    /// <summary> �����X�|�[���J�[������܂ł̒x������ </summary>
    [SerializeField, CustomLabel("�����X�|�[���J�[������܂ł̒x������")]
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
            // ���X�|�[���̃C���^�[�o���҂�����
            if (m_respownList[i].m_IsRespawnInterval)
            {
                m_respownList[i].m_FrameCount--;
                if (m_respownList[i].m_FrameCount <= 0.0f)
                {
                    // ���X�|�[���J�n
                    StartRespawn(m_respownList[i].m_Player);
                    m_respownList[i].m_IsRespawnInterval = false;
                }
            }
        }

        if (m_IsSetRespawnAll)
        {
            // �J�n�̉��o�����邽�ߒx��������
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
            "���X�|�[��_�N���N�V����",
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
