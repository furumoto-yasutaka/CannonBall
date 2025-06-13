using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpMove_Avatar : PlayerSpMove
{
    /// <summary> ���g�̃v���n�u </summary>
    [SerializeField, CustomLabel("���g�̃v���n�u")]
    private GameObject m_clonePrefab;

    /// <summary> ���g�̐� </summary>
    [SerializeField, CustomLabel("���g�̐�")]
    private int m_cloneNum = 3;

    /// <summary> ���g����яo�����x </summary>
    [SerializeField, CustomLabel("���g����яo�����x(�ŏ��A�ő�)")]
    private Vector2 m_cloneInitSpeed = Vector2.zero;

    /// <summary> ���g�I�u�W�F�N�g </summary>
    private GameObject[] m_cloneObj;

    private Rigidbody2D m_rb;


    public int m_CloneNum { get { return m_cloneNum; } }
    
    public GameObject[] m_CloneObj { get { return m_cloneObj; } }


    protected override void Start()
    {
        base.Start();

        m_rb = GetComponent<Rigidbody2D>();
        m_cloneObj = new GameObject[m_cloneNum];
    }

    /// <summary> �K�E�Z�𔭓����� </summary>
    public override void StartSpMove()
    {
        // ���g�𐶐�
        Quaternion q;
        float angle = 360 / m_cloneNum;
        for (int i = 0; i < m_cloneNum; i++)
        {
            q = Quaternion.identity;
            q *= Quaternion.AngleAxis(angle * i, Vector3.forward);
            m_cloneObj[i] = Instantiate(m_clonePrefab, transform.position, q);
            m_cloneObj[i].GetComponent<Rigidbody2D>().velocity =
                (Vector2)m_cloneObj[i].transform.up.normalized * Random.Range(m_cloneInitSpeed.x, m_cloneInitSpeed.y);
            m_cloneObj[i].GetComponent<OriginInfo>().InitParam(GetComponent<PlayerId>().m_Id, m_playerController, m_playerImpact, m_playerPoint);
            m_cloneObj[i].GetComponent<PlayerImpact_Clone>().InitParam();
        }
        m_rb.gravityScale = 1.5f;

        AudioManager.Instance.PlaySe("�L���m���t�@�C�g_���g", false);

        base.StartSpMove();
    }

    /// <summary> �K�E�Z���I������ </summary>
    public override void EndSpMove()
    {
        base.EndSpMove();

        // �f�B�]���u�A�j���[�V�������J�n
        for (int i = 0; i < m_cloneNum; i++)
        {
            m_cloneObj[i].GetComponent<CloneController>().StartDestroy();
        }

        m_rb.gravityScale = 0.5f;
    }
}
