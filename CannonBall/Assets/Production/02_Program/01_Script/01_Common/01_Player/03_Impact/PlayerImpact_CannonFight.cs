using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_CannonFight : PlayerImpact
{
    /// <summary> �v���C���[�K�E�Z�Ǘ��R���|�[�l���g </summary>
    protected PlayerSpMove m_playerSpMove;

    /// <summary> �n�ʂ��R���(�ޔ�p) </summary>
    private float m_tempKickPlatformPower;

    /// <summary> �G���R���(�ޔ�p) </summary>
    private float m_tempKickPlayerPower;

    /// <summary> �R����������󂯐g�s��Ԃɂ��邩(�ޔ�p) </summary>
    private bool m_tempIsNotPassiveKick;

    /// <summary> �󂯐g�s�ɂ��鎞��(�ޔ�p) </summary>
    private float m_tempNotPassiveTime;


    protected override void Start()
    {
        base.Start();

        m_playerSpMove = GetComponent<PlayerSpMove>();
    }

    /// <summary>
    /// �󂯐g�s��Ԃɂ���
    /// </summary>
    /// <param name="time"> �󂯐g�s��Ԃ̎��� </param>
    public override void SetNotPassive(float time)
    {
        if (!m_playerSpMove.m_IsSpMove && 
            m_notPassiveTimeCount < time)
        {
            m_isNotPassive = true;
            m_notPassiveTimeCount = time;
        }
    }

    /// <summary> �p�����[�^��K�E�Z�p�ɕύX </summary>
    public void SetDuringSpMoveParam()
    {
        // �n�ʂ��R���
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlatformPower)
        {
            m_tempKickPlatformPower = m_kickPlatformPower;
            m_kickPlatformPower = m_playerSpMove.m_DuringSpMove_KickPlatformPower;
        }
        // �G���R���
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlayerPower)
        {
            m_tempKickPlayerPower = m_kickPlayerPower;
            m_kickPlayerPower = m_playerSpMove.m_DuringSpMove_KickPlayerPower;
        }
        // �R����������󂯐g�s��Ԃɂ��邩
        if (m_playerSpMove.m_IsChangeDuringSpMove_IsNotPassiveKick)
        {
            m_tempIsNotPassiveKick = m_isNotPassiveKick;
            m_isNotPassiveKick = m_playerSpMove.m_DuringSpMove_IsNotPassiveKick;
        }
        // �󂯐g�s�ɂ��鎞��
        if (m_playerSpMove.m_IsChangeDuringSpMove_NotPassiveTime)
        {
            m_tempNotPassiveTime = m_notPassiveTime;
            m_notPassiveTime = m_playerSpMove.m_DuringSpMove_NotPassiveTime;
        }
    }

    /// <summary> �p�����[�^���f�t�H���g�l�ɕύX </summary>
    public void SetDefaultParam()
    {
        // �n�ʂ��R���
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlatformPower)
        {
            m_kickPlatformPower = m_tempKickPlatformPower;
        }
        // �G���R���
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickPlayerPower)
        {
            m_kickPlayerPower = m_tempKickPlayerPower;
        }
        // �R����������󂯐g�s��Ԃɂ��邩
        if (m_playerSpMove.m_IsChangeDuringSpMove_IsNotPassiveKick)
        {
            m_isNotPassiveKick = m_tempIsNotPassiveKick;
        }
        // �󂯐g�s�ɂ��鎞��
        if (m_playerSpMove.m_IsChangeDuringSpMove_NotPassiveTime)
        {
            m_notPassiveTime = m_tempNotPassiveTime;
        }
    }
}
