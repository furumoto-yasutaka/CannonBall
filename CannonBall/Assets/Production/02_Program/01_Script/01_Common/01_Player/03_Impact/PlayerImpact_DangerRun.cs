using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_DangerRun : PlayerImpact
{
    /// <summary> リスポーン時に飛び出す時の力 </summary>
    [SerializeField, CustomLabel("リスポーン時に飛び出す時の力")]
    protected float m_kickRespawnPower = 10.0f;


    /// <summary>
    /// リスポーンから出る時の蹴りの処理
    /// </summary>
    public void KickRespawn()
    {
        m_rb.velocity = CalcImpactPlatform(-m_playerController.m_KickDir, m_kickRespawnPower);

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }
}
