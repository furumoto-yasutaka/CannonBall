using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_CannonFight_Avatar : PlayerController_CannonFight
{
    private PlayerSpMove_Avatar m_playerSpMove_Avatar;


    protected override void Awake()
    {
        base.Awake();

        m_playerSpMove_Avatar = GetComponent<PlayerSpMove_Avatar>();
    }

    protected override void MoveUpdate()
    {
        float vec = m_rb.velocity.x;
        base.MoveUpdate();

        if (m_playerSpMove_Avatar.m_IsSpMove)
        {
            vec = m_rb.velocity.x - vec;
            for (int i = 0; i < m_playerSpMove_Avatar.m_CloneNum; i++)
            {
                m_playerSpMove_Avatar.m_CloneObj[i].GetComponent<CloneController>().Move(vec);
            }
        }
    }

    protected override void KickUpdate()
    {
        bool kick = m_isKicking;
        bool spMove = m_playerSpMove_Avatar.m_IsSpMove;
        base.KickUpdate();

        if (spMove && m_playerSpMove_Avatar.m_IsSpMove)
        {
            if (!kick && m_isKicking)
            {
                for (int i = 0; i < m_playerSpMove_Avatar.m_CloneNum; i++)
                {
                    m_playerSpMove_Avatar.m_CloneObj[i].GetComponent<CloneController>().Kick(m_kickDir);
                }
            }
        }
    }
}
