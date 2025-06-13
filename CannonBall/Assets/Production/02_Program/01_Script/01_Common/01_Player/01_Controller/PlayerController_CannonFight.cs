using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_CannonFight : PlayerController
{
    [SerializeField, CustomLabel("�J�����^�[�Q�b�g�O���[�v�o�^")]
    private CinemachineTargetGroupRegister m_targetGroupResister;

    /// <summary> �v���C���[�K�E�Z�Ǘ��R���|�[�l���g </summary>
    private PlayerSpMove m_playerSpMove;

    /// <summary> �R��_�˂��o������(�ޔ�p) </summary>
    private float m_tempKickStickOutSpeed;

    /// <summary> �R��_�Ђ����߂鑬��(�ޔ�p) </summary>
    private float m_tempKickRetractSpeed;


    protected override void Awake()
    {
        base.Awake();

        m_playerSpMove = GetComponent<PlayerSpMove>();
    }

    protected override void PlayAction()
    {
        base.PlayAction();

        // �K�E�Z�����ɂ��Ă̍X�V
        SpMoveUpdate();
    }

    private void SpMoveUpdate()
    {
        if (((PlayerInputController_CannonFight)m_inputController).GetSpMove(m_playerId.m_Id))
        {
            // �K�E�Z�Q�[�W��MAX�ɂȂ��Ă��邩
            if (m_playerSpMove.m_IsSpMovePointMaxCharge)
            {
                // �K�E�Z�𔭓�����
                m_playerSpMove.StartSpMove();
            }
        }
    }

    protected override void RevivalUpdate()
    {
        base.KickUpdate();
        if (m_isKicking)
        {
            Revival();
            m_legCollision.enabled = false;
            // �R���ďo�Ă��铮��
            m_playerImpact.KickPlatform();
        }
        else
        {
            m_revivalTimeCount -= Time.deltaTime;
            if (m_revivalTimeCount <= 0.0f)
            {
                m_revivalTimeCount = 0.0f;
                Revival();
            }
        }
    }

    /// <summary> �p�����[�^��K�E�Z�p�ɕύX </summary>
    public void SetDuringSpMoveParam()
    {
        // �����o�鑬��
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickStickOutSpeed)
        {
            m_tempKickStickOutSpeed = m_kickStickOutSpeed;
            m_kickStickOutSpeed = m_playerSpMove.m_DuringSpMove_KickStickOutSpeed;
            m_animator.SetFloat("StickOutSpeed", m_kickStickOutSpeed);
        }
        // ����߂�����
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickRetractSpeed)
        {
            m_tempKickRetractSpeed = m_kickRetractSpeed;
            m_kickRetractSpeed = m_playerSpMove.m_DuringSpMove_KickRetractSpeed;
            m_animator.SetFloat("RetractSpeed", m_kickRetractSpeed);
        }
    }

    /// <summary> �p�����[�^���f�t�H���g�l�ɕύX </summary>
    public void SetDefaultParam()
    {
        // �����o�鑬��
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickStickOutSpeed)
        {
            m_kickStickOutSpeed = m_tempKickStickOutSpeed;
            m_animator.SetFloat("StickOutSpeed", m_kickStickOutSpeed);
        }
        // ����߂�����
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickRetractSpeed)
        {
            m_kickRetractSpeed = m_tempKickRetractSpeed;
            m_animator.SetFloat("RetractSpeed", m_kickRetractSpeed);
        }
    }

    protected override void Revival()
    {
        base.Revival();

        m_respawnAnimator.SetBool("IsFirstRespawn", false);
        m_targetGroupResister.Resist(transform);
    }

    public override void Death(float revivalTime)
    {
        base.Death(revivalTime);

        m_targetGroupResister.Delete(transform);
    }

    public void FirstDeath()
    {
        // �X�e�[�g�����S�ɕύX
        SetState(State.Death);
        // �R���W�����𖳌��ɂ���
        m_colController.DisableCollider();
        // �������Z�n���~
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // ���X�|�[���{�b�N�X��L���ɂ���
        m_respawnBox.SetActive(true);
        // �����܂ł̎��Ԃ�ݒ�
        m_revivalTimeCount = 0.0f;
    }

    public void InitCameraTargetGroup(CinemachineTargetGroupRegister targetGroup)
    {
        m_targetGroupResister = targetGroup;
    }
}
