/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�ւ̏Ռ��𐧌䂷��X�N���v�g(�T�b�J�[���[�hver)
*	�t�@�C���F	PlayerImpact_Succer.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_Succer : PlayerImpact
{
    /// <summary> �{�[�����R��� </summary>
    // �{���̓{�[�����ɍ�����̕ϐ����������A
    // �Ⴄ�\�͂̃L������I������@�\���J�����ꂽ���ׁ̈A
    // �v���C���[���ɒ�`����
    [SerializeField, CustomLabel("�{�[�����R���")]
    private float m_kickBallPower = 1.0f;

    /// <summary> �{�[�����R�������̔��� </summary>
    [SerializeField, CustomLabel("�{�[�����R�������̔���")]
    private float m_kickBallRecoil = 1.0f;


    public float m_KickBallPower { get { return m_kickBallPower; } }


    /// <summary> �{�[���R�莞�̔����̔��f </summary>
    public void KickBallRecoil()
    {
        // ���x���v�Z�E���f
        m_rb.velocity = -m_playerController.m_KickDir * m_kickBallRecoil;

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }
}
