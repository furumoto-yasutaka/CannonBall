/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̑��̃R���W�����C�x���g�X�N���v�g(�T�b�J�[���[�hver)
*	�t�@�C���F	PlayerLegOnCollision_Succer.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision_Succer : PlayerLegOnCollision
{
    /// <summary> �v���C���[���͎�t </summary>
    private PlayerImpact_Succer m_playerImpact_Succer;


    protected override void Start()
    {
        base.Start();

        m_playerImpact_Succer = transform.root.GetComponent<PlayerImpact_Succer>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Ball"))
        {// �{�[�����R����
            OnTriggerEnter_Ball(other);
        }
    }

    /// <summary> �{�[���ɐG�ꂽ�ۂ̃R���W�����C�x���g </summary>
    /// <param name="other"> �{�[���̃R���W���� </param>
    private void OnTriggerEnter_Ball(Collider2D other)
    {
        m_playerImpact_Succer.KickBallRecoil();
        other.transform.root.GetComponent<BallImpact>().Kicked(
            m_playerController.m_KickDir,
            m_playerImpact_Succer.m_KickBallPower,
            m_rb.velocity);
    }
}
