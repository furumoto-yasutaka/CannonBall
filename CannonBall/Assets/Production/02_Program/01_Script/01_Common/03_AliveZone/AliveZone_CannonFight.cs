/*******************************************************************************
*
*	�^�C�g���F	�v���C���[���������Ă��邩���f����X�N���v�g(�嗐�����[�hver)
*	�t�@�C���F	AliveZone_CannonFight.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AliveZone_CannonFight : AliveZone
{
    private CinemachineImpulseSource m_impulseSource;

    private bool m_stop = false;


    private void Start()
    {
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary> �v���C���[�����ꂽ�ۂ̃C�x���g </summary>
    /// <param name="collision"> �v���C���[�̃R���W���� </param>
    protected override void OnTriggerExit_Player(Collider2D collision)
    {
        if (m_stop) { return; }

        Transform parent = collision.transform.root;
        int id = parent.GetComponent<PlayerId>().m_Id;

        if (collision.transform.position.x < 0.0f)
        {
            transform.GetChild(0).GetChild(id).GetComponent<ParticleSystem>().Play();
        }
        else
        {
            transform.GetChild(1).GetChild(id).GetComponent<ParticleSystem>().Play();
        }

        base.OnTriggerExit_Player(collision);

        // �L�����ꂽ���Ƃɂ��|�C���g�̐��Z���s��
        parent.GetComponent<PlayerPoint_CannonFight>().KilledDividePoint();

        PlayerSpMove spMove = parent.GetComponent<PlayerSpMove>();
        // �K�E�Z��������������I������
        if (spMove.m_IsSpMove)
        {
            spMove.EndSpMove();
            spMove.ResetSpMovePoint();
        }
        // �L�����ꂽ���Ƃɂ��K�E�Z�Q�[�W�̑������s��
        spMove.AccumulateBeKilledPattern();

        // ��ʂ���炷
        m_impulseSource.GenerateImpulse();

        AudioManager.Instance.PlaySe(
            "�L���m���t�@�C�g_��O���̉�",
            false);
        AudioManager.Instance.PlaySe(
            "�L���m���t�@�C�g_��O���̊���",
            false);

        // �R���g���[���[�U��
        VibrationManager.Instance.SetVibration(id, 30, 0.9f);
    }

    public void Stop()
    {
        m_stop = true;
    }
}
