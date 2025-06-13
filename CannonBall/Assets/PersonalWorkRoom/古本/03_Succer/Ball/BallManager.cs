/*******************************************************************************
*
*	�^�C�g���F	�{�[�����Ǘ�����V���O���g���X�N���v�g
*	�t�@�C���F	BallManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : SingletonMonoBehaviour<BallManager>
{
    /// <summary> �{�[���̃X�|�[�����ɂ��� </summary>
    [System.Serializable]
    public class BallSpawnInfo
    {
        public int ChangeSecond = 0;
        public int BallSpawnMax = 0;
    }

    /// <summary> ���A�{�[���̃X�|�[���m���ɂ��� </summary>
    [System.Serializable]
    public class RareBallSpownRate
    {
        public int ChangeSecond = 0;
        public float RareBallSpawnRate = 0.0f;
    }

    /// <summary> �e�{�[���̏�� </summary>
    [System.Serializable]
    public class BallInfo
    {
        public GameObject Ball = null;
        public float SpownInterval = 0.0f;
    }

    /// <summary> �ʏ�{�[���̃v���n�u </summary>
    [SerializeField, CustomLabel("�ʏ�{�[���̃v���n�u")]
    private GameObject m_ballPrefab;

    /// <summary> ���{�[���̃v���n�u </summary>
    [SerializeField, CustomLabel("���{�[���̃v���n�u")]
    private GameObject m_rareballPrefab;

    /// <summary> �{�[���̏o���ʒu </summary>
    [SerializeField, CustomLabel("�{�[���̏o���ʒu")]
    private Vector3 m_ballSpownPos;

    /// <summary> �{�[���̏o���ʒu���� </summary>
    [SerializeField, CustomLabel("�{�[���̏o���ʒu����")]
    private Vector2 m_ballSpownPosOffset;

    [Header("�{�[���̓����o������Ɋւ�����")]
    /// <summary> �{�[���̓����o������Ɋւ����� </summary>
    [SerializeField]
    private List<BallSpawnInfo> m_ballSpawnInfoList;

    [Header("���{�[���̏o�����Ɋւ�����")]
    /// <summary> ���{�[���̏o�����Ɋւ����� </summary>
    [SerializeField]
    private List<RareBallSpownRate> m_rareBallSpownRateList;

    [Header("���{�[�����o�����Ȃ������玟�m��ŏo���������̕b��")]
    /// <summary> ���{�[�����o�����Ȃ������玟�m��ŏo���������̕b�� </summary>
    [SerializeField]
    private List<int> m_rareBallCompensatingTimeList;

    /// <summary> �ő剽�A���܂ŋ��{�[���̏o���������� </summary>
    [SerializeField, CustomLabel("�ő剽�A���܂ŋ��{�[���̏o����������")]
    private int m_rareBallConsecutivsMax;

    /// <summary> 
    /// �{�[��������Ȃ��Ȃ�A�ǉ������܂ł̃N�[���^�C��
    /// ���{�[���̓����o������������Ȃ��Ă��̋󂫂�ǉ�����ꍇ�̓N�[���^�C������
    /// </summary>
    [SerializeField, CustomLabel("�{�[���ǉ��܂ł̃N�[���^�C��")]
    private int m_ballSpownCooltime;

    /// <summary> �t�B�[���h�ɑ��݂��Ă���{�[���̏�� </summary>
    [SerializeField]
    private List<BallInfo> m_balls = new List<BallInfo>();

    /// <summary> �{�[���̓����o����� </summary>
    [SerializeField, CustomLabelReadOnly("�{�[���̓����o�����")]
    private int m_ballSpawnMax = 1;

    /// <summary> ���{�[���o���m�� </summary>
    [SerializeField, CustomLabelReadOnly("���{�[���o���m��")]
    private float m_rareBallSpawnRate = 0.5f;

    /// <summary> �w�莞�ԓ��ɋ��{�[�����o�������� </summary>
    [SerializeField, CustomLabelReadOnly("�w�莞�ԓ��ɋ��{�[�����o��������")]
    private bool m_isRareBallSpawn = false;

    /// <summary> ���̃{�[���͋��{�[���m�肩 </summary>
    [SerializeField, CustomLabelReadOnly("���̃{�[���͋��{�[���m�肩")]
    private bool m_isNextSpownRareBall = false;

    /// <summary> ���{�[���̘A���o���� </summary>
    [SerializeField, CustomLabelReadOnly("���{�[���̘A���o����")]
    private int m_rareBallConsecutiveCount = 0;


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        // �C���X�y�N�^�[�̏���b���̍~���ɕ��ёւ�
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

        // �N�[���^�C�����m�F���A�{�[����ǉ�����
        CheckBallSpownInterval();
    }

    /// <summary>
    /// ���̃{�[���̏o�����X�V
    /// </summary>
    private void CheckRareBallSpownRate()
    {
        if (m_rareBallSpownRateList.Count > 0)
        {
            // �w�莞�Ԍo�߂��Ă�����X�V
            if (m_rareBallSpownRateList[0].ChangeSecond >= Timer.Instance.m_TimeCounter)
            {
                m_rareBallSpawnRate = m_rareBallSpownRateList[0].RareBallSpawnRate;
                m_rareBallSpownRateList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// ���{�[���m��o���ɂ��čX�V
    /// </summary>
    private void CheckRareBallCompensatingTime()
    {
        if (m_rareBallCompensatingTimeList.Count > 0)
        {
            // �w�莞�Ԍo�߂��Ă�����X�V
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
    /// �{�[���̓����o�����X�V
    /// </summary>
    private void CheckBallSpawnInfo()
    {
        if (m_ballSpawnInfoList.Count > 0)
        {
            // �w�莞�Ԍo�߂��Ă�����X�V
            if (m_ballSpawnInfoList[0].ChangeSecond >= Timer.Instance.m_TimeCounter)
            {
                m_ballSpawnMax = m_ballSpawnInfoList[0].BallSpawnMax;
                m_ballSpawnInfoList.RemoveAt(0);
                AddBallInfo(m_ballSpawnMax);
            }
        }
    }

    /// <summary>
    /// �ŏ��̃{�[���𐶐�����
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
    /// �{�[���̓����o���������ɔ����{�[���̘g��ǉ�����
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
    /// �{�[���𐶐�
    /// </summary>
    private GameObject CreateBall()
    {
        GameObject ball;
        
        if (m_isNextSpownRareBall)
        {// ���{�[���m��
            ball = Instantiate(m_rareballPrefab);
            m_isNextSpownRareBall = false;
        }
        else if (m_rareBallConsecutiveCount >= m_rareBallConsecutivsMax)
        {// ���{�[���̘A���o������ɒB���Ă���
            ball = Instantiate(m_ballPrefab);
            m_rareBallConsecutiveCount = 0;
        }
        else
        {// �����Ō���
            float rand = Random.Range(0.0f, 1.0f);
            if (rand >= m_rareBallSpawnRate)
            {// �ʏ�{�[������
                ball = Instantiate(m_ballPrefab);
                m_rareBallConsecutiveCount = 0;
            }
            else
            {// ���{�[������
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
    /// �{�[�����폜
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
