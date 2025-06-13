using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_Clone : PlayerImpact
{
    private OriginInfo m_originInfo;


    protected override void Start() { }

    protected override void Update() { }

    public void InitParam()
    {
        m_originInfo = GetComponent<OriginInfo>();
        m_rb = GetComponent<Rigidbody2D>();
        m_playerController = m_originInfo.m_PlayerController;
        m_kickPlatformPower = m_originInfo.m_PlayerImpact.m_KickPlatformPower;
        m_kickPlayerPower = m_originInfo.m_PlayerImpact.m_KickPlayerPower;
        m_kickPlayerRecoil = m_originInfo.m_PlayerImpact.m_KickPlayerRecoil;
        m_kickAngularPower = m_originInfo.m_PlayerImpact.m_KickAngularPower;
        m_inertiaPowerRate_Platform = m_originInfo.m_PlayerImpact.m_InertiaPowerRate_Platform;
        m_inertiaPowerRate_Player = m_originInfo.m_PlayerImpact.m_InertiaPowerRate_Player;
        m_isNotPassiveKick = m_originInfo.m_PlayerImpact.m_IsNotPassiveKick;
        m_notPassiveTime = m_originInfo.m_PlayerImpact.m_NotPassiveTime;
    }
}
