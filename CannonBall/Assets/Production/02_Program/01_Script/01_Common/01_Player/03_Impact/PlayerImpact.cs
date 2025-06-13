/*******************************************************************************
*
*	�^�C�g���F	�v���C���[��������ԏ������܂Ƃ߂��X�N���v�g
*	�t�@�C���F	PlayerImpact.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/12
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact : MonoBehaviour
{
    /// <summary> �n�`���R��� </summary>
    [SerializeField, CustomLabel("�n�`���R���")]
    protected float m_kickPlatformPower = 1.0f;

    /// <summary> ���v���C���[���R��� </summary>
    [SerializeField, CustomLabel("���v���C���[���R���")]
    protected float m_kickPlayerPower = 1.0f;

    /// <summary> ���v���C���[���R�����Ƃ��̔��� </summary>
    [SerializeField, CustomLabel("���v���C���[���R�����Ƃ��̔���")]
    protected float m_kickPlayerRecoil = 1.0f;

    /// <summary> �Ԃ��Ƃ񂾂Ƃ��ɂ���������x1������̉�]�����x </summary>
    [SerializeField, CustomLabel("�Ԃ��Ƃ񂾂Ƃ��ɂ���������x1������̉�]�����x")]
    protected float m_kickAngularPower = 100.0f;

    /// <summary> �n�`���R�����Ƃ��ɂ���������x1������̉�]�����x </summary>
    [SerializeField, CustomLabel("�����x1������̒n�`�R�莞�̒ǉ����x����")]
    protected Vector2 m_inertiaPowerRate_Platform;

    /// <summary> �v���C���[���R�����Ƃ��ɂ���������x1������̉�]�����x </summary>
    [SerializeField, CustomLabel("�����x1������̑��v���C���[�R�莞�̒ǉ����x����")]
    protected float m_inertiaPowerRate_Player;

    /// <summary> �R����������󂯐g�s��Ԃɂ��邩 </summary>
    [SerializeField, CustomLabel("�R����������󂯐g�s��Ԃɂ��邩")]
    protected bool m_isNotPassiveKick = false;

    /// <summary> �󂯐g�s��Ԃɂ��鎞�� </summary>
    [SerializeField, CustomLabel("�󂯐g�s��Ԃɂ��鎞��")]
    protected float m_notPassiveTime = 1.0f;

    /// <summary> �������󂯐g�s��Ԃ��ǂ��� </summary>
    [SerializeField, CustomLabelReadOnly("�������󂯐g�s��Ԃ��ǂ���")]
    protected bool m_isNotPassive = false;

    /// <summary> ���˂��̋���(��b�l) </summary>
    [SerializeField, CustomLabel("���˂��̋���(��b�l)")]
    protected float m_headbuttBasePower = 3.0f;

    /// <summary> �����̑��x�ɉ��������˂��̉��Z���� </summary>
    [SerializeField, CustomLabel("�����̑��x�ɉ��������˂��̋����̉��Z����")]
    protected float m_headbuttAddRate = 0.2f;

    /// <summary> �Ԃ����ł��鎞�ɕ��o����p�[�e�B�N���̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�Ԃ����ł��鎞�ɕ��o����p�[�e�B�N���̐e�I�u�W�F�N�g")]
    protected Transform m_blownawayEffObj;

    /// <summary> �Ԃ����ł��鎞�ɕ��o����p�[�e�B�N�����������x��臒l </summary>
    [SerializeField, CustomLabel("�Ԃ����ł��鎞�ɕ��o����p�[�e�B�N�����������x��臒l")]
    protected float m_blownawayEffHideThreshold = 5.0f;

    /// <summary> �Ԃ����ł��鎞�ɐU�������鑬�x��臒l </summary>
    [SerializeField, CustomLabel("�Ԃ����ł��鎞�ɐU�������鑬�x��臒l")]
    protected float m_vibrationThreshold = 5.0f;

    /// <summary> �Ԃ����ł��鎞�ɐU�������鑬�x��臒l </summary>
    [SerializeField, CustomLabel("�Ԃ����ł��鎞�ɍő�ɐU�������鑬�x��臒l")]
    protected float m_vibrationMaxThreshold = 25.0f;

    /// <summary> ���W�b�h�{�f�B </summary>
    protected Rigidbody2D m_rb;

    /// <summary> �v���C���[���͎�t </summary>
    protected PlayerController m_playerController;

    /// <summary> �v���C���[���G���� </summary>
    protected PlayerInvincible m_playerInvincible;

    /// <summary> �󂯐g�s��Ԃ̎c�莞�� </summary>
    protected float m_notPassiveTimeCount = 0.0f;

    /// <summary> �Ԃ����ł��鎞�ɕ��o����p�[�e�B�N�� </summary>
    protected RotateLock_Custom m_blownawayRotateSetter;

    /// <summary> �v���C���[�\���R���|�[�l���g </summary>
    protected PlayerFaceController m_faceController;

    protected bool m_isBlownVibration = false;


    public float m_KickPlatformPower { get { return m_kickPlatformPower; } }

    public float m_KickPlayerPower { get { return m_kickPlayerPower; } }

    public float m_KickPlayerRecoil { get { return m_kickPlayerRecoil; } }

    public float m_KickAngularPower { get { return m_kickAngularPower; } }

    public Vector2 m_InertiaPowerRate_Platform { get { return m_inertiaPowerRate_Platform; } }
    
    public float m_InertiaPowerRate_Player { get { return m_inertiaPowerRate_Player; } }

    public bool m_IsNotPassiveKick { get { return m_isNotPassiveKick; } }

    public float m_NotPassiveTime { get { return m_notPassiveTime; } }

    public bool m_IsNotPassive { get { return m_isNotPassive; } }

    public float m_HeadbuttBasePower { get { return m_headbuttBasePower; } }

    public float m_HeadbuttAddRate { get { return m_headbuttAddRate; } }


    protected virtual void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_playerController = GetComponent<PlayerController>();
        m_playerInvincible = GetComponent<PlayerInvincible>();
        m_blownawayRotateSetter = m_blownawayEffObj.GetComponent<RotateLock_Custom>();
        m_faceController = GetComponent<PlayerFaceController>();
    }

    protected virtual void Update()
    {
        // �󂯐g�s��Ԃɂ��Ă̍X�V����
        NotPassiveTimeUpdate();

        // �v���C���[���Ԃ���΂��ꂽ���̃G�t�F�N�g���o�������邩�m�F����
        CheckBlownAwayEff();
        CheckVibration();
    }

    /// <summary>
    /// �R��̈З͂��v�Z
    /// </summary>
    /// <param name="dir"> ���ł������� </param>
    /// <param name="power"> �L�b�N�� </param>
    protected Vector2 CalcImpactPlatform(Vector2 dir, float power)
    {
        Vector2 baseVel = dir * power;
        Vector2 resultVel = baseVel;

        if (baseVel.x > 0)
        {
            resultVel.x += Mathf.Abs(m_rb.velocity.x) * m_inertiaPowerRate_Platform.x;
        }
        else
        {
            resultVel.x -= Mathf.Abs(m_rb.velocity.x) * m_inertiaPowerRate_Platform.x;
        }

        // ���݂̃v���C���[�̏㉺���x���W�����v�̏㉺�ւ̉����x��
        // ��������ԂɂȂ�悤�Ƀx�N�g�����v�Z����
        if (m_rb.velocity.y > 0 && baseVel.y > 0)
        {
            resultVel.y += Mathf.Abs(m_rb.velocity.y) * m_inertiaPowerRate_Platform.y;
        }

        return resultVel;
    }

    /// <summary>
    /// �R��̈З͂��v�Z
    /// </summary>
    /// <param name="dir"> ���ł������� </param>
    /// <param name="power"> �L�b�N�� </param>
    /// <param name="vel"> ���� </param>
    private Vector2 CalcImpactKicked(Vector2 dir, float power, Vector2 vel)
    {
        Vector2 baseVel = dir * power;
        Vector2 resultVel = baseVel;

        resultVel += dir * vel.magnitude * m_inertiaPowerRate_Player;

        return resultVel;
    }

    /// <summary>
    /// �n�`���R�鏈��
    /// </summary>
    public void KickPlatform()
    {
        m_rb.velocity = CalcImpactPlatform(-m_playerController.m_KickDir, m_kickPlatformPower);

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }

    /// <summary>
    /// �v���C���[���R���������̏���
    /// </summary>
    public void KickPlayerRecoil()
    {
        // ���x���v�Z�E���f
        m_rb.velocity = -m_playerController.m_KickDir * m_kickPlayerRecoil;

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }

    /// <summary>
    /// �v���C���[�ɏR��ꂽ����
    /// </summary>
    public virtual void Kicked(Vector2 dir, float power, Vector2 vel)
    {
        // ���G��Ԃ̎��͏Ռ���^���Ȃ�
        if (m_playerInvincible.m_IsInvincible) { return; }

        // ���x���v�Z�E���f
        m_rb.velocity = CalcImpactKicked(dir, power, vel);

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;

        m_blownawayEffObj.GetChild(0).GetComponent<ParticleSystem>().Play();
        m_blownawayRotateSetter.SetRotate(dir);

        m_isBlownVibration = true;

        m_faceController.SetHitFace(true);
    }

    /// <summary>
    /// �v���C���[�ɓ��˂��ꂽ����
    /// </summary>
    public void Headbutted(Vector2 dir, float power, Vector2 vel)
    {
        // ���G��Ԃ̎��͏Ռ���^���Ȃ�
        if (m_playerInvincible.m_IsInvincible) { return; }

        // ���x���v�Z�E���f
        Vector2 resultVel = dir * power;
        resultVel += dir * vel.magnitude * m_headbuttAddRate;
        m_rb.velocity = resultVel;

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }

    /// <summary>
    /// �R����������󂯐g�s��Ԃɂ��邩�ǂ����ݒ�
    /// </summary>
    /// <param name="isActive"> �R����������󂯐g�s��Ԃɂ��邩 </param>
    public void NotPassiveKick(bool isActive)
    {
        m_isNotPassiveKick = isActive;
    }

    /// <summary>
    /// �󂯐g�s��Ԃɂ��Ă̍X�V����
    /// </summary>
    public void NotPassiveTimeUpdate()
    {
        if (m_isNotPassive)
        {
            m_notPassiveTimeCount -= Time.deltaTime;
            if (m_notPassiveTimeCount <= 0.0f)
            {
                m_notPassiveTimeCount = 0.0f;
                m_isNotPassive = false;
            }
        }
    }

    /// <summary>
    /// �󂯐g�s��Ԃɂ���
    /// </summary>
    /// <param name="time"> �󂯐g�s��Ԃ̎��� </param>
    public virtual void SetNotPassive(float time)
    {
        if (m_notPassiveTimeCount < time)
        {
            m_isNotPassive = true;
            m_notPassiveTimeCount = time;
        }
    }

    /// <summary>
    /// �v���C���[���Ԃ���΂��ꂽ���̃G�t�F�N�g���o�������邩�m�F
    /// </summary>
    public void CheckBlownAwayEff()
    {
        ParticleSystem ps = m_blownawayEffObj.GetChild(0).GetComponent<ParticleSystem>();

        if (!ps.isPlaying) { return; }

        // ��葬�x�ȉ���������G�t�F�N�g���B������߂�
        if (m_rb.velocity.sqrMagnitude <= m_blownawayEffHideThreshold * m_blownawayEffHideThreshold)
        {
            m_faceController.SetHitFace(false);
            ps.Stop();
        }
    }

    public void CheckVibration()
    {
        if (!m_isBlownVibration) { return; }

        float sqrThreshold = m_vibrationThreshold * m_vibrationThreshold;
        if (m_rb.velocity.sqrMagnitude <= sqrThreshold)
        {
            m_isBlownVibration = false;
        }
        else
        {
            float sqrMaxThreshold = m_vibrationMaxThreshold * m_vibrationMaxThreshold;
            float p = Mathf.Clamp(
                m_rb.velocity.sqrMagnitude,
                sqrThreshold,
                sqrMaxThreshold);
            p -= sqrThreshold;
            p /= sqrMaxThreshold - sqrThreshold;
            VibrationManager.Instance.SetVibration(GetComponent<PlayerId>().m_Id, 1, p);
        }
    }
}
