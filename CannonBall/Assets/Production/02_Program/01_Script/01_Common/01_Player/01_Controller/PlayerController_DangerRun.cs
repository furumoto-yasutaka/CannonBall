using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_DangerRun : PlayerController
{
    private AnimationFloatParamLerp m_animationFloatParamLerp_RespawnSwitch;

    private AnimationFloatParamLerp m_animationFloatParamLerp_PlayersCount;

    private bool m_isActive = false;


    public AnimationFloatParamLerp m_AnimationFloatParamLerp_RespawnSwitch
        { get { return m_animationFloatParamLerp_RespawnSwitch; } }

    public AnimationFloatParamLerp m_AnimationFloatParamLerp_PlayersCount
        { get { return m_animationFloatParamLerp_PlayersCount; } }


    protected override void Awake()
    {
        base.Awake();

        m_animationFloatParamLerp_RespawnSwitch = 
            new AnimationFloatParamLerp(m_respawnAnimator, "RespawnSwitch", 0.0f, 0.0f);
        m_animationFloatParamLerp_PlayersCount =
            new AnimationFloatParamLerp(m_respawnAnimator, "PlayersCount", 0.0f, 0.0f);
    }

    protected override void Update()
    {
        if (!m_isActive) { return; }

        base.Update();

        m_animationFloatParamLerp_RespawnSwitch.Update();
        m_animationFloatParamLerp_PlayersCount.Update();
    }

    protected override void PlayAction()
    {
        // �ړ��ɂ��Ă̍X�V
        MoveUpdate();
        // �R��ɂ��Ă̍X�V
        KickUpdate();
        // ���˂��ɂ��Ă̍X�V
        HeadbuttUpdate();
        // �̂̉�]�Ɏ����Ă�����Ȃ��悤�ɑ��̕�����␳����
        m_legParent.transform.rotation =
            Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_kickDir), Vector3.forward);

        if (m_revivalNextFrame)
        {
            // �R���ďo�Ă��铮��
            ((PlayerImpact_DangerRun)m_playerImpact).KickRespawn();
            m_revivalNextFrame = false;
        }
    }

    protected override void ResetPosition()
    {
        Vector3 pos = transform.GetChild(0).position;
        transform.parent = null;
        transform.GetChild(0).position = Vector3.zero;
        transform.position = pos;
    }

    public override void Death(float revivalTime)
    {
        // �X�e�[�g�����S�ɕύX
        SetState(State.Death);
        // �R���W�����𖳌��ɂ���
        m_colController.DisableCollider();
        // �������Z�n�̍��W�n���~
        m_rb.velocity = Vector3.zero;
        m_rb.constraints = RigidbodyConstraints2D.FreezePosition;
        // �R����I������
        DisableLegCallback();
        // ���˂����I������
        m_isHeadbutt = false;
        // �����܂ł̎��Ԃ�ݒ�
        m_revivalTimeCount = revivalTime;
    }

    public void StopAndWarp()
    {
        // �������Z�n���~
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // ���W�������Ȃ��ʒu�Ɉړ�
        transform.position = new Vector3(0.0f, -100.0f, 0.0f);
        // ���X�|�[���{�b�N�X��L���ɂ���
        m_respawnBox.SetActive(true);
    }

    public void Active()
    {
        m_isActive = true;
    }
}
