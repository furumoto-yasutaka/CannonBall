using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_DangerRun : PlayerImpact
{
    /// <summary> ���X�|�[�����ɔ�яo�����̗� </summary>
    [SerializeField, CustomLabel("���X�|�[�����ɔ�яo�����̗�")]
    protected float m_kickRespawnPower = 10.0f;


    /// <summary>
    /// ���X�|�[������o�鎞�̏R��̏���
    /// </summary>
    public void KickRespawn()
    {
        m_rb.velocity = CalcImpactPlatform(-m_playerController.m_KickDir, m_kickRespawnPower);

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }
}
