using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpMove_WallShockWave : PlayerSpMove
{
    [Header("�K�E�Z�ŗL�p�����[�^")]

    /// <summary> �Ռ��g�𔭓�������x�N�g����臒l </summary>
    [SerializeField, CustomLabel("�Ռ��g�𔭓�������x�N�g����臒l")]
    private float m_wallShockWaveActivationThreshold = 1.0f;

    /// <summary> �Ռ��g�ɂ�萁����ԋ��� </summary>
    [SerializeField, CustomLabel("�Ռ��g�ɂ�萁����ԋ���")]
    private float m_wallShockWavePower = 3.0f;

    /// <summary> �Ռ��g�̓����蔻�蔭������ </summary>
    [SerializeField, CustomLabel("�Ռ��g�̓����蔻�蔭������")]
    private float m_wallShockWaveActivateTime = 0.3f;

    /// <summary> �����Ŏ����łԂ���ԕ����̃p�^�[���� </summary>
    [SerializeField, CustomLabel("�����Ŏ����łԂ���ԕ����̃p�^�[����")]
    private int m_wallShockWaveDirPattern = 32;

    /// <summary> �����������łԂ���ԋ��� </summary>
    [SerializeField, CustomLabel("�����������łԂ���ԋ���")]
    private float m_bouncePower = 12.0f;

    /// <summary> ����Ɉ��G�v���C���[�Ɍ����Ĕ��ł����� </summary>
    [SerializeField, CustomLabel("����Ɉ��G�v���C���[�Ɍ����Ĕ��ł�����")]
    private int m_bounceToPlayerFreq = 3;

    /// <summary> �̂̃R���W�����R�[���o�b�N </summary>
    [SerializeField, CustomLabel("�̂̃R���W�����R�[���o�b�N")]
    private PlayerBodyOnCollision m_bodyCollisionCallback;

    private Vector2 m_bounceVelocity = Vector2.zero;

    private Rigidbody2D m_rb;

    private List<int> m_wallShockWaveDirPatternList = new List<int>();

    private bool m_isSetSpMoveVelocity = false;

    private int m_bounceCount = 0;

    private List<PlayerController> m_otherPlayers = new List<PlayerController>();


    public float m_WallShockWaveActivationThreshold { get { return m_wallShockWaveActivationThreshold; } }

    public float m_WallShockWavePower { get { return m_wallShockWavePower; } }

    public float m_WallShockWaveActivateTime { get { return m_wallShockWaveActivateTime; } }


    protected override void Start()
    {
        base.Start();

        m_rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < m_wallShockWaveDirPattern; i++)
        {
            m_wallShockWaveDirPatternList.Add(i);
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p != gameObject)
            {
                m_otherPlayers.Add(p.GetComponent<PlayerController>());
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (m_isSpMove && m_isSetSpMoveVelocity)
        {
            m_rb.velocity = m_bounceVelocity;
        }
    }

    /// <summary> �K�E�Z�𔭓����� </summary>
    public override void StartSpMove()
    {
        base.StartSpMove();

        Bounce();
        m_rb.mass = 0.01f;
        m_playerController.SetBumperIgnore(true);
        m_bodyCollisionCallback.SetPlayBounceSe(false);
    }

    /// <summary> �K�E�Z���I������ </summary>
    public override void EndSpMove()
    {
        base.EndSpMove();

        m_rb.mass = 1.0f;
        m_playerController.SetBumperIgnore(false);
        m_bodyCollisionCallback.SetPlayBounceSe(true);
    }

    public void Bounce()
    {
        m_bounceCount = (m_bounceCount + 1) % m_bounceToPlayerFreq;

        if (m_bounceCount == 0)
        {
            Bounce_Player();
        }
        else
        {
            Bounce_Random();
        }
    }

    private void Bounce_Random()
    {
        float rotate = 360.0f / m_wallShockWaveDirPattern;
        int i = 0;
        Vector2 dir = Vector2.up;
        for (; i < m_wallShockWaveDirPattern; i++)
        {
            bool end = true;
            int rand = Random.Range(0, m_wallShockWaveDirPattern - 1 - i);
            int pattern = m_wallShockWaveDirPatternList[rand];
            m_wallShockWaveDirPatternList.RemoveAt(rand);
            m_wallShockWaveDirPatternList.Add(pattern);
            dir = Quaternion.Euler(0, 0, rotate * pattern) * Vector2.up;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.5f, dir, 30.0f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("ShockWaveWall"))
                {
                    end = false;
                    break;
                }
                else if (hit.collider.CompareTag("Platform") && hit.distance < 2.5f)
                {
                    end = false;
                    break;
                }
            }

            if (end)
            {
                break;
            }
        }

        m_isSetSpMoveVelocity = true;
        if (i < m_wallShockWaveDirPattern)
        {
            m_bounceVelocity = dir * m_bouncePower;
        }
        else
        {
            int rand = Random.Range(0, m_wallShockWaveDirPattern - 1);
            int pattern = m_wallShockWaveDirPatternList[rand];
            dir = Quaternion.Euler(0, 0, rotate * pattern) * Vector2.up;
            m_bounceVelocity = dir * m_bouncePower;
        }
        m_rb.velocity = m_bounceVelocity;
    }

    private void Bounce_Player()
    {
        // ��ԋ߂��G����������
        Vector2 minDir = Vector2.zero;
        float minSqrLen = float.MaxValue;

        foreach (PlayerController t in m_otherPlayers)
        {
            if (t.m_State != PlayerController.State.Play) { continue; }

            Vector2 dist = t.transform.position - transform.position;
            float len = dist.sqrMagnitude;
            // ����ōŒZ��
            if (len < minSqrLen)
            {
                
                // �W�I�̌�낪��O��������L�^���Ȃ�
                bool assign = true;
                RaycastHit2D[] hits = Physics2D.CircleCastAll(t.transform.position, 0.5f, dist.normalized, 30.0f);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("ShockWaveWall"))
                    {
                        assign = false;
                        break;
                    }
                }
                if (assign)
                {
                    minSqrLen = len;
                    minDir = dist.normalized;
                }
            }
        }

        // �L���ȃv���C���[��������Ȃ�����
        if (minSqrLen == float.MaxValue)
        {
            // �d���Ȃ��̂Ń����_���ȕ����ɔ��
            Bounce_Random();
            // �v���C���[�Ɍ����Ĕ�ׂȂ������̂�
            // �����v���C���[�Ɍ����Ĕ��ł�����Ԃɂ���
            m_bounceCount = m_bounceToPlayerFreq - 1;
        }
        else
        {
            m_isSetSpMoveVelocity = true;
            m_bounceVelocity = minDir * m_bouncePower;
            m_rb.velocity = m_bounceVelocity;
        }
    }

    public void DisableIsSetSpMoveVelocity()
    {
        m_isSetSpMoveVelocity = false;
    }
}
