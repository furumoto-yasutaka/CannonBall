using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpMove_Avatar : PlayerSpMove
{
    /// <summary> 分身体プレハブ </summary>
    [SerializeField, CustomLabel("分身体プレハブ")]
    private GameObject m_clonePrefab;

    /// <summary> 分身の数 </summary>
    [SerializeField, CustomLabel("分身の数")]
    private int m_cloneNum = 3;

    /// <summary> 分身が飛び出す速度 </summary>
    [SerializeField, CustomLabel("分身が飛び出す速度(最小、最大)")]
    private Vector2 m_cloneInitSpeed = Vector2.zero;

    /// <summary> 分身オブジェクト </summary>
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

    /// <summary> 必殺技を発動する </summary>
    public override void StartSpMove()
    {
        // 分身を生成
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

        AudioManager.Instance.PlaySe("キャノンファイト_分身", false);

        base.StartSpMove();
    }

    /// <summary> 必殺技を終了する </summary>
    public override void EndSpMove()
    {
        base.EndSpMove();

        // ディゾルブアニメーションを開始
        for (int i = 0; i < m_cloneNum; i++)
        {
            m_cloneObj[i].GetComponent<CloneController>().StartDestroy();
        }

        m_rb.gravityScale = 0.5f;
    }
}
