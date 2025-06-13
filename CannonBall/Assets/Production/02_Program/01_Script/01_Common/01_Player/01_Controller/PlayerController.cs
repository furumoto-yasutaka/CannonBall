/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̓��͂��󂯕t����X�N���v�g
*	�t�@�C���F	PlayerController.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/12
*
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Death = 0,
        Respawn,
        Play,
    }

    public enum Type
    {
        Balance = 0,
        Power,
        Speed,
    }

    #region �ϐ�

    [Header("�Q��")]
    /// <summary> ���I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("���I�u�W�F�N�g")]
    protected GameObject m_leg;
    /// <summary> ���̓����蔻��I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("���̓����蔻��I�u�W�F�N�g")]
    protected Collider2D m_legCollision;
    /// <summary> ���̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("���̐e�I�u�W�F�N�g")]
    protected GameObject m_legParent;
    /// <summary> �̂̓����蔻��I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�̂̓����蔻��I�u�W�F�N�g")]
    protected Collider2D m_bodyCollision;
    /// <summary> ���X�|�[���{�b�N�X </summary>
    [SerializeField, CustomLabel("���X�|�[���{�b�N�X")]
    protected GameObject m_respawnBox;
    /// <summary> �R�莞�̃G�t�F�N�g�̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�R�莞�̃G�t�F�N�g�̐e�I�u�W�F�N�g")]
    protected Transform m_kickEffectParent;
    /// <summary> �v���C���[�^�C�v </summary>
    [SerializeField, CustomLabel("�v���C���[�^�C�v")]
    protected Type m_type = Type.Balance;

    [Header("�R��֌W")]
    /// <summary> �R����� </summary>
    [SerializeField, CustomLabelReadOnly("�R�����")]
    protected Vector2 m_kickDir = Vector2.zero;
    /// <summary> �R��_�˂��o������(�b) </summary>
    [SerializeField, CustomLabel("�R��_�˂��o������(�{)")]
    protected float m_kickStickOutSpeed = 0.05f;
    /// <summary> �R��_�Ђ����߂鑬��(�b) </summary>
    [SerializeField, CustomLabel("�R��_�Ђ����߂鑬��(�{)")]
    protected float m_kickRetractSpeed = 0.1f;
    /// <summary> ���˂���Ԃ̍Œ��p������ </summary>
    [SerializeField, CustomLabel("���˂���Ԃ̍Œ��p������")]
    protected float m_headbuttTime = 1.0f;
    /// <summary> ���˂���Ԃ��p�����鑬�x </summary>
    [SerializeField, CustomLabel("���˂���Ԃ��p�����鑬�x")]
    protected float m_headbuttKeepSpeed = 2.0f;
    /// <summary> ���˂���Ԃ��p�����鑬�x </summary>
    [SerializeField, CustomLabel("���˂����̎���")]
    protected float m_headbuttMass = 5.0f;

    [Header("�ړ��֌W")]
    /// <summary> �ړ����x </summary>
    [SerializeField, CustomLabel("�ړ������x")]
    protected float m_moveSpeed = 2.0f;
    /// <summary> ���͂����݂̈ړ������Ƌt�̎��̌������x </summary>
    [SerializeField, CustomLabel("���͂����݂̈ړ������Ƌt�̎��̌������x")]
    protected float m_moveDecaySpeed = 0.8f;
    /// <summary> ���͂ɂ��ő�ړ����x </summary>
    [SerializeField, CustomLabel("���͂ɂ��ő�ړ����x")]
    protected float m_moveSpeedMax = 2.5f;

    [Header("���̑�")]
    /// <summary> ������̖��G���� </summary>
    [SerializeField, CustomLabel("������̖��G����")]
    protected float m_revivalInvincible = 2.0f;
    /// <summary> �o���p�[�𖳎����邩 </summary>
    [SerializeField, CustomLabel("�o���p�[�𖳎����邩")]
    protected bool m_bumperIgnore = false;

    [Header("�f�o�b�O�p")]
    /// <summary> �X�e�[�g </summary>
    [SerializeField, CustomLabel("�X�e�[�g")]
    protected State m_state = State.Play;

    /// <summary> �e�^�C�v�̓��{�ꖼ </summary>
    public static readonly string[] m_TypeStr =
    {
        "�o�����X",
        "�p���[",
        "�X�s�[�h",
    };

    /// <summary> �A�j���[�^�[�R���|�[�l���g </summary>
    protected Action[] m_stateAction;
    /// <summary> �A�j���[�^�[�R���|�[�l���g </summary>
    protected Animator m_animator;
    /// <summary> ���X�|�[���p�A�j���[�^�[�R���|�[�l���g </summary>
    protected Animator m_respawnAnimator;
    /// <summary> ���W�b�h�{�f�B�R���|�[�l���g </summary>
    protected Rigidbody2D m_rb;
    /// <summary> �v���C���[�̔ԍ� </summary>
    protected PlayerId m_playerId;
    /// <summary> �v���C���[�ւ̏Ռ��Ǘ��R���|�[�l���g </summary>
    protected PlayerImpact m_playerImpact;
    /// <summary> �v���C���[�̖��G�Ǘ��R���|�[�l���g </summary>
    protected PlayerInvincible m_playerInvincible;
    /// <summary> �v���C���[�̑��̔���R�[���o�b�N </summary>
    protected PlayerLegOnCollision m_playerLegOnCollision;
    /// <summary> �R���W��������R���|�[�l���g </summary>
    protected CollisionController m_colController;
    /// <summary> �v���C���[���͎擾�p�R���|�[�l���g </summary>
    protected PlayerInputController m_inputController;
    /// <summary> �v���C���[�\���R���|�[�l���g </summary>
    protected PlayerFaceController m_faceController;

    /// <summary> �R���Ă��邩�ǂ��� </summary>
    protected bool m_isKicking = false;
    /// <summary> ���˂����Ă��邩�ǂ��� </summary>
    protected bool m_isHeadbutt = false;
    /// <summary> ���˂��̎c�莞�� </summary>
    protected float m_headbuttTimeCount = 0.0f;
    /// <summary> ���͕s���ǂ��� </summary>
    protected bool m_isNotInputReceive = false;
    /// <summary> ���������܂ł̐������� </summary>
    protected float m_revivalTimeCount = 0.0f;
    /// <summary> �����������̃t���[�����ǂ��� </summary>
    protected bool m_revivalNextFrame = false;

    #endregion

    public Type m_Type { get { return m_type; } }

    public float m_KickStickOutSpeed { get { return m_kickStickOutSpeed; } }

    public float m_KickRetractSpeed { get { return m_kickRetractSpeed; } }

    public bool m_IsKicking { get { return m_isKicking; } }

    public State m_State { get { return m_state; } }

    public Vector2 m_KickDir { get { return m_kickDir; } }

    public bool m_BumperIgnore { get { return m_bumperIgnore; } }

    public bool m_IsHeadbutt { get { return m_isHeadbutt; } }


    protected virtual void Awake()
    {
        m_stateAction = new Action[]{ DeathAction, RespawnAction, PlayAction };

        m_animator = GetComponent<Animator>();
        m_respawnAnimator = transform.GetChild(0).GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_playerId = GetComponent<PlayerId>();
        m_playerImpact = GetComponent<PlayerImpact>();
        m_playerInvincible = GetComponent<PlayerInvincible>();
        m_playerLegOnCollision = m_legCollision.GetComponent<PlayerLegOnCollision>();
        m_colController = GetComponent<CollisionController>();
        m_inputController = transform.parent.GetComponent<PlayerInputController>();
        m_faceController = GetComponent<PlayerFaceController>();

        // �R��̑������A�j���[�V�����R���g���[���[�ɔ��f
        m_animator.SetFloat("StickOutSpeed", m_kickStickOutSpeed);
        m_animator.SetFloat("RetractSpeed", m_kickRetractSpeed);

        // �����A�N�e�B�u��
        // ���V�[����ł��Ȃ����R�͏������ő����Q�Ƃ��邱�Ƃ����邽��
        m_leg.SetActive(false);
    }

    protected virtual void Update()
    {
        m_stateAction[(int)m_state]();
    }

    private void DeathAction() {}

    private void RespawnAction()
    {
        // �����ɂ��Ă̍X�V
        RevivalUpdate();
    }

    protected virtual void PlayAction()
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
            m_playerImpact.KickPlatform();
            m_revivalNextFrame = false;
        }
    }

    /// <summary>
    /// �X�e�B�b�N��|�������ǂ������f����(Pad)
    /// </summary>
    private bool IsStickTilt()
    {
        return m_inputController.GetKick(m_playerId.m_Id);
    }

    #region �X�V����

    /// <summary>
    /// �ړ��֘A�̍X�V
    /// </summary>
    protected virtual void MoveUpdate()
    {
        // ���͎�t�s��Ԃ̏ꍇ�ړ����͂��󂯕t���Ȃ�
        if (m_isNotInputReceive) { return; }

        // ���͎擾
        float horizontal = m_inputController.GetMove(m_playerId.m_Id).x;

        // �ʏ�̑��x�v�Z
        float defaultSpeed = m_moveSpeed * horizontal * Time.deltaTime;
        // ��������ꍇ�̑��x�v�Z
        float decaySpeed = m_rb.velocity.x * m_moveDecaySpeed * horizontal * Time.deltaTime;

        float addSpeed = defaultSpeed;
        if (horizontal < 0.0f && m_rb.velocity.x >= -m_moveSpeedMax)
        {
            // �l�̑傫�������x(�����������)�𔽉f����
            if (decaySpeed < defaultSpeed)
            {
                addSpeed = decaySpeed;
            }
        }
        else if (horizontal > 0.0f && m_rb.velocity.x <= m_moveSpeedMax)
        {
            // �l�̑傫�������x(�����������)�𔽉f����
            if (decaySpeed > defaultSpeed)
            {
                addSpeed = decaySpeed;
            }
        }

        // ���x���f
        m_rb.velocity += new Vector2(addSpeed, 0.0f);
    }

    /// <summary>
    /// �R��֘A�̍X�V
    /// </summary>
    protected virtual void KickUpdate()
    {
        // �󂯐g�s��Ԃ܂��͓��͎�t�s��Ԃ̏ꍇ�R����͂��󂯕t���Ȃ�
        if (m_playerImpact.m_IsNotPassive || m_isNotInputReceive || m_isKicking) { return; }

        bool isKickStart = false;

        if (IsStickTilt())
        {// Pad
            // �R������X�V
            m_kickDir = m_inputController.GetKickDir(m_playerId.m_Id).normalized;

            isKickStart = true;
        }

        if (isKickStart)
        {
            // �R����J�n
            m_leg.SetActive(true);
            m_legCollision.enabled = true;
            m_animator.SetTrigger("Kick");
            m_isKicking = true;
            EffectContainer.Instance.EffectPlay(
                "�R�蕗�؂�_" + m_TypeStr[(int)m_type],
                m_kickEffectParent.position,
                m_kickEffectParent.rotation,
                m_kickEffectParent,
                transform.localScale);
            AudioManager.Instance.PlaySe(
                "�v���C���[_�R��̕��؂艹_" + m_TypeStr[(int)m_type],
                false);
            AudioManager.Instance.PlaySe(
                "�v���C���[_�R��̃z���O������_" + m_TypeStr[(int)m_type],
                false);
            m_faceController.SetAngryFace(true);
        }
    }

    /// <summary>
    /// ���˂��֘A�̍X�V
    /// </summary>
    protected void HeadbuttUpdate()
    {
        if (!m_isHeadbutt) { return; }

        m_headbuttTimeCount -= Time.deltaTime;
        if (m_headbuttTimeCount <= 0.0f ||
            m_rb.velocity.sqrMagnitude < m_headbuttKeepSpeed * m_headbuttKeepSpeed)
        {
            EndHeadbutt();
        }
    }

    protected virtual void RevivalUpdate()
    {
        KickUpdate();
        if (m_isKicking)
        {
            m_legCollision.enabled = false;
            Revival();
            m_revivalNextFrame = true;
            return;
        }

        m_revivalTimeCount -= Time.deltaTime;
        if (m_revivalTimeCount <= 0.0f)
        {
            m_revivalTimeCount = 0.0f;
            Revival();
            return;
        }
    }

    #endregion

    #region ���̑�

    public void SetState(State next)
    {
        m_state = next;
    }

    public void SetBumperIgnore(bool balue)
    {
        m_bumperIgnore = balue;
    }

    /// <summary>
    /// �R��̓����蔻��폜�R�[���o�b�N
    /// </summary>
    public void DisableLegCollisionCallback()
    {
        m_legCollision.enabled = false;
    }

    /// <summary>
    /// �R��I�����R�[���o�b�N
    /// </summary>
    public void DisableLegCallback()
    {
        // �R����I������
        m_leg.SetActive(false);
        m_isKicking = false;
        // ���̏Փˍς݃��X�g�����Z�b�g����
        m_playerLegOnCollision.ResetContactList();
        m_faceController.SetAngryFace(false);
    }

    /// <summary>
    /// ���˂��J�n����
    /// </summary>
    public void StartHeadbutt()
    {
        // ���˂��̊J�n
        m_isHeadbutt = true;
        m_rb.mass = m_headbuttMass;
        m_headbuttTimeCount = m_headbuttTime;
    }

    public void EndHeadbutt()
    {
        m_isHeadbutt = false;
        m_rb.mass = 1;
        m_headbuttTimeCount = 0.0f;
    }

    /// <summary>
    /// ���X�|�[���{�b�N�X�ړ������R�[���o�b�N
    /// </summary>
    public void JumpOutFinishCallback_Respawn()
    {
        // �X�e�[�g�����X�|�[���҂��ɕύX
        SetState(State.Respawn);
    }

    /// <summary>
    /// ���X�|�[���{�b�N�X�ړ������R�[���o�b�N
    /// </summary>
    public void JumpOutFinishCallback_FirstRespawn()
    {
        // ����������
        Revival();
    }

    /// <summary>
    /// ���͎�t�s��Ԃ�ύX����
    /// </summary>
    /// <param name="isNotInputReceive"> ���͎�t�s�ɂ��邩�ǂ��� </param>
    public void SetIsNotInputReceive(bool isNotInputReceive)
    {
        m_isNotInputReceive = isNotInputReceive;
    }

    protected virtual void Revival()
    {
        // �X�e�[�g��ʏ푀��ɕύX
        SetState(State.Play);
        // �R���W������L���ɂ���
        m_colController.EnableCollider();
        // �������Z�n���ċN��
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.None;
        // ���X�|�[���{�b�N�X�𖳌��ɂ���
        m_respawnBox.SetActive(false);
        // �A�j���[�V�����̃��X�|�[���ҋ@���I������
        m_respawnAnimator.SetBool("IsRespawn", false);
        // ���G���Ԃ�ݒ�
        m_playerInvincible.SetInvincible(m_revivalInvincible);
        // ���������̂Ń��X�|�[���}�l�[�W���[�ɏI���ʒm
        RespawnManager.Instance.EndRespawn(transform);
        // ���W�֌W�����ɖ߂�
        ResetPosition();
    }

    protected virtual void ResetPosition()
    {
        transform.root.position = transform.GetChild(0).position;
        transform.root.rotation = transform.GetChild(0).rotation;
        transform.GetChild(0).localPosition = Vector3.zero;
        transform.GetChild(0).localRotation = Quaternion.identity;
    }

    public virtual void Death(float revivalTime)
    {
        // �X�e�[�g�����S�ɕύX
        SetState(State.Death);
        // �R���W�����𖳌��ɂ���
        m_colController.DisableCollider();
        // �������Z�n���~
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // ���W�������Ȃ��ʒu�Ɉړ�
        transform.position = new Vector3(0.0f, -100.0f, 0.0f);
        // �R����I������
        DisableLegCallback();
        // ���˂����I������
        m_isHeadbutt = false;
        // ���X�|�[���{�b�N�X��L���ɂ���
        m_respawnBox.SetActive(true);
        // �����܂ł̎��Ԃ�ݒ�
        m_revivalTimeCount = revivalTime;
    }

    #endregion
}
