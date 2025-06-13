using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision_Clone : PlayerLegOnCollision
{
    private OriginInfo m_originInfo;


    protected override void Start()
    {
        Transform p = transform.root;
        m_rb = p.GetComponent<Rigidbody2D>();
        m_originInfo = p.GetComponent<OriginInfo>();
        m_playerController = m_originInfo.m_PlayerController;
        m_playerImpact = p.GetComponent<PlayerImpact_Clone>();
    }

    /// <summary> プレイヤーが触れた際のイベント </summary>
    /// <param name="other"> プレイヤーのコリジョン </param>
    protected override void OnTriggerEnter_Player(Collider2D other)
    {
        // 自身の本体に当たった場合は無視する
        if (transform.root.GetComponent<PlayerId>().m_Id == other.transform.root.GetComponent<PlayerId>().m_Id) { return; }

        base.OnTriggerEnter_Player(other);

        other.transform.root.GetComponent<PlayerPoint_CannonFight>().RequestKickMark(m_originInfo.m_PlayerPoint);
    }
}
