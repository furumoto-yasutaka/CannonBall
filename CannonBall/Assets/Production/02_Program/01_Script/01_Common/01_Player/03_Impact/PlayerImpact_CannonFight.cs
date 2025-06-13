using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_CannonFight : PlayerImpact
{
    /// <summary> プレイヤー必殺技管理コンポーネント </summary>
    protected PlayerSpMove m_playerSpMove;

    /// <summary> 地面を蹴る力(退避用) </summary>
    private float m_tempKickPlatformPower;

    /// <summary> 敵を蹴る力(退避用) </summary>
    private float m_tempKickPlayerPower;

    /// <summary> 蹴った相手を受け身不可状態にするか(退避用) </summary>
    private bool m_tempIsNotPassiveKick;

    /// <summary> 受け身不可にする時間(退避用) </summary>
    private float m_tempNotPassiveTime;


    protected override void Start()
    {
        base.Start();

        m_playerSpMove = GetComponent<PlayerSpMove>();
    }

    /// <summary>
    /// 受け身不可状態にする
    /// </summary>
    /// <param name="time"> 受け身不可状態の時間 </param>
    public override void SetNotPassive(float time)
    {
        if (!m_playerSpMove.m_IsSpMove && 
            m_notPassiveTimeCount < time)
        {
            m_isNotPassive = true;
            m_notPassiveTimeCount = time;
        }
    }

    /// <summary> パラメータを必殺技用に変更 </summary>
    public void SetDuringSpMoveParam()
    {
        // 地面を蹴る力
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlatformPower)
        {
            m_tempKickPlatformPower = m_kickPlatformPower;
            m_kickPlatformPower = m_playerSpMove.m_DuringSpMove_KickPlatformPower;
        }
        // 敵を蹴る力
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlayerPower)
        {
            m_tempKickPlayerPower = m_kickPlayerPower;
            m_kickPlayerPower = m_playerSpMove.m_DuringSpMove_KickPlayerPower;
        }
        // 蹴った相手を受け身不可状態にするか
        if (m_playerSpMove.m_IsChangeDuringSpMove_IsNotPassiveKick)
        {
            m_tempIsNotPassiveKick = m_isNotPassiveKick;
            m_isNotPassiveKick = m_playerSpMove.m_DuringSpMove_IsNotPassiveKick;
        }
        // 受け身不可にする時間
        if (m_playerSpMove.m_IsChangeDuringSpMove_NotPassiveTime)
        {
            m_tempNotPassiveTime = m_notPassiveTime;
            m_notPassiveTime = m_playerSpMove.m_DuringSpMove_NotPassiveTime;
        }
    }

    /// <summary> パラメータをデフォルト値に変更 </summary>
    public void SetDefaultParam()
    {
        // 地面を蹴る力
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlatformPower)
        {
            m_kickPlatformPower = m_tempKickPlatformPower;
        }
        // 敵を蹴る力
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlayerPower)
        {
            m_kickPlayerPower = m_tempKickPlayerPower;
        }
        // 蹴った相手を受け身不可状態にするか
        if (m_playerSpMove.m_IsChangeDuringSpMove_IsNotPassiveKick)
        {
            m_isNotPassiveKick = m_tempIsNotPassiveKick;
        }
        // 受け身不可にする時間
        if (m_playerSpMove.m_IsChangeDuringSpMove_NotPassiveTime)
        {
            m_notPassiveTime = m_tempNotPassiveTime;
        }
    }
}
