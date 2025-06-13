using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    /// <summary> ���I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("���I�u�W�F�N�g")]
    private GameObject m_leg;
    /// <summary> ���̓����蔻��I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("���̓����蔻��I�u�W�F�N�g")]
    private Collider2D m_legCollision;
    /// <summary> ���̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("���̐e�I�u�W�F�N�g")]
    private GameObject m_legParent;
    /// <summary> �R�莞�̃G�t�F�N�g�̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�R�莞�̃G�t�F�N�g�̐e�I�u�W�F�N�g")]
    protected Transform m_kickEffectParent;

    private OriginInfo m_originInfo;
    /// <summary> �A�j���[�^�[�R���|�[�l���g </summary>
    private Animator m_animator;
    /// <summary> ���W�b�h�{�f�B�R���|�[�l���g </summary>
    private Rigidbody2D m_rb;
    /// <summary> �v���C���[�̑��̔���R�[���o�b�N </summary>
    private PlayerLegOnCollision m_playerLegOnCollision;
    /// <summary> �v���C���[�\���R���|�[�l���g </summary>
    private PlayerFaceController m_faceController;
    /// <summary> �R����� </summary>
    private Vector2 m_kickDir = Vector2.zero;


    private void Start()
    {
        m_originInfo = GetComponent<OriginInfo>();
        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_playerLegOnCollision = m_legCollision.GetComponent<PlayerLegOnCollision>();
        m_faceController = GetComponent<PlayerFaceController>();

        // �R��̑������A�j���[�V�����R���g���[���[�ɔ��f
        m_animator.SetFloat("StickOutSpeed", m_originInfo.m_PlayerController.m_KickStickOutSpeed);
        m_animator.SetFloat("RetractSpeed", m_originInfo.m_PlayerController.m_KickRetractSpeed);
    }

    private void Update()
    {
        // �̂̉�]�Ɏ����Ă�����Ȃ��悤�ɑ��̕�����␳����
        m_legParent.transform.rotation =
            Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_kickDir), Vector3.forward);
    }

    public void Move(float addSpeed)
    {
        // ���x���f
        m_rb.velocity += new Vector2(addSpeed, 0.0f);
    }

    public void Kick(Vector2 kickDir)
    {
        m_kickDir = kickDir;
        // �R����J�n
        m_leg.SetActive(true);
        m_legCollision.enabled = true;
        m_animator.SetTrigger("Kick");

        string type = PlayerController.m_TypeStr[(int)m_originInfo.m_PlayerController.m_Type];
        EffectContainer.Instance.EffectPlay(
            "�R�蕗�؂�_" + type,
            m_kickEffectParent.position,
            m_kickEffectParent.rotation,
            m_kickEffectParent);
        m_faceController.SetAngryFace(true);
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
        // ���̏Փˍς݃��X�g�����Z�b�g����
        m_playerLegOnCollision.ResetContactList();
        m_faceController.SetAngryFace(false);
    }

    /// <summary>
    /// ���ŊJ�n
    /// </summary>
    public void StartDestroy()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("IsDestroy", true);
    }
}
