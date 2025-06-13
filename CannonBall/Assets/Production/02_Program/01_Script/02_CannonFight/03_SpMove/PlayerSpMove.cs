using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerSpMove : MonoBehaviour
{
    #region �K�E�Z�|�C���g�֘A�p�����[�^

    /// <summary> �K�E�Z�|�C���g�ő�l </summary>
    [SerializeField, CustomLabel("�K�E�Z�|�C���g�ő�l")]
    private float m_spMovePointMax = 100.0f;

    /// <summary> �K�E�Z�|�C���g </summary>
    [SerializeField, CustomLabel("�K�E�Z�|�C���g")]
    private float m_spMovePoint = 0.0f;

    [Header("�R��ꂽ��")]
    /// <summary> �R��ꂽ���ɗ��܂�悤�ɂ��邩 </summary>
    [SerializeField, CustomLabel("�L���ɂ��邩")]
    private bool m_isUseBeKickedPattern;

    /// <summary> �R��ꂽ���̃Q�[�W�̏㏸�� </summary>
    [SerializeField, CustomLabel("�㏸��")]
    private float m_bekickedPatternRateOfUp;

    [Header("�G�v���C���[���R������")]
    /// <summary> �G�v���C���[���R�������ɗ��܂�悤�ɂ��邩 </summary>
    [SerializeField, CustomLabel("�L���ɂ��邩")]
    private bool m_isUseKickPattern;

    /// <summary> �G�v���C���[���R�������̃Q�[�W�̏㏸�� </summary>
    [SerializeField, CustomLabel("�㏸��")]
    private float m_kickPatternRateOfUp;

    [Header("�L�����ꂽ��")]
    /// <summary> �L�����ꂽ���ɗ��܂�悤�ɂ��邩 </summary>
    [SerializeField, CustomLabel("�L���ɂ��邩")]
    private bool m_isUseBeKilledPattern;

    /// <summary> �L�����ꂽ���̃Q�[�W�̏㏸�� </summary>
    [SerializeField, CustomLabel("�㏸��")]
    private float m_bekilledPatternRateOfUp = 10.0f;

    [Header("���Ԍo��")]
    /// <summary> ���Ԍo�߂ɗ��܂�悤�ɂ��邩 </summary>
    [SerializeField, CustomLabel("�L���ɂ��邩")]
    private bool m_isUseTimePattern;

    /// <summary> ���Ԍo�߂̃Q�[�W�̏㏸��(0.1�b��) </summary>
    [SerializeField, CustomLabel("�㏸��(0.1�b��)")]
    private float m_timePatternRateOfUp = 0.05f;

    /// <summary> ���Ԍo�߂̃Q�[�W�̏㏸�� </summary>
    [SerializeField, CustomArrayLabel(new string[] { "1��","2��","3��","4��", })]
    private float[] m_timePatternRankMag = new float[] { 1.0f, 1.25f, 1.5f, 2.0f, };

    #endregion

    #region �K�E�Z���p�����[�^

    [Header("�K�E�Z���p�����[�^")]

    /// <summary> �K�E�Z�����\�G�t�F�N�g </summary>
    [SerializeField, CustomLabel("�K�E�Z�����\�G�t�F�N�g")]
    private GameObject m_isCanSpMoveEffect;

    /// <summary> �K�E�Z�����\�ɂȂ����u�Ԃ̃G�t�F�N�g </summary>
    [SerializeField, CustomLabel("�K�E�Z�����\�ɂȂ����u�Ԃ̃G�t�F�N�g")]
    private Transform m_spmoveChargeEffect;

    /// <summary> �K�E�Z�������� </summary>
    [SerializeField, CustomLabel("��������")]
    private float m_spMoveActivationTime;

    [Space(8)]
    /// <summary> ����˂��o��������ω����邩 </summary>
    [SerializeField, CustomLabel("����˂��o��������ω����邩")]
    private bool m_isChangeDuringSpMove_KickStickOutSpeed;
    /// <summary> ����˂��o������ </summary>
    [SerializeField, CustomLabel("����˂��o������")]
    private float m_duringSpMove_KickStickOutSpeed;

    [Space(8)]
    /// <summary> ����߂�������ω����邩 </summary>
    [SerializeField, CustomLabel("����߂�������ω����邩")]
    private bool m_isChangeDuringSpMove_KickRetractSpeed;
    /// <summary> ����߂����� </summary>
    [SerializeField, CustomLabel("����߂�����")]
    private float m_duringSpMove_KickRetractSpeed;

    [Space(8)]
    /// <summary> �n�ʂ��R��͂�ω����邩 </summary>
    [SerializeField, CustomLabel("�n�ʂ��R��͂�ω����邩")]
    private bool m_isChangeDuringSpMove_KickPlatformPower;
    /// <summary> �n�ʂ��R��� </summary>
    [SerializeField, CustomLabel("�n�ʂ��R���")]
    private float m_duringSpMove_KickPlatformPower;

    [Space(8)]
    /// <summary> �G���R��͂�ω����邩 </summary>
    [SerializeField, CustomLabel("�G���R��͂�ω����邩")]
    private bool m_isChangeDuringSpMove_KickPlayerPower;
    /// <summary> �G���R��� </summary>
    [SerializeField, CustomLabel("�G���R���")]
    private float m_duringSpMove_KickPlayerPower;

    [Space(8)]
    /// <summary> �R����������󂯐g�s��Ԃɂ��邩��ω����邩 </summary>
    [SerializeField, CustomLabel("�R����������󂯐g�s��Ԃɂ��邩��ω����邩")]
    private bool m_isChangeDuringSpMove_IsNotPassiveKick;
    /// <summary> �R����������󂯐g�s��Ԃɂ��邩 </summary>
    [SerializeField, CustomLabel("�R����������󂯐g�s��Ԃɂ��邩")]
    private bool m_duringSpMove_IsNotPassiveKick;

    [Space(8)]
    /// <summary> �󂯐g�s�ɂ��鎞�Ԃ�ω����邩 </summary>
    [SerializeField, CustomLabel("�󂯐g�s�ɂ��鎞�Ԃ�ω����邩")]
    private bool m_isChangeDuringSpMove_NotPassiveTime;
    /// <summary> �󂯐g�s�ɂ��鎞�� </summary>
    [SerializeField, CustomLabel("�󂯐g�s�ɂ��鎞��")]
    private float m_duringSpMove_NotPassiveTime;

    #endregion

    #region �v���C�x�[�g�����o�ϐ�

    /// <summary> �K�E�Z�����ʑ����^�C�����ǂ��� </summary>
    public static bool m_IsChargeAddition = false;

    /// <summary> �K�E�Z������2�{�^�C�����ǂ��� </summary>
    protected static float m_isChargeAdditionRate = 2.0f;

    /// <summary> �K�E�Z�Q�[�W�����܂肫���Ă��邩 </summary>
    protected bool m_isSpMovePointMaxCharge = false;

    /// <summary> �K�E�Z���������ǂ��� </summary>
    protected bool m_isSpMove = false;

    /// <summary> �K�E�Z���������ǂ��� </summary>
    protected float m_spMoveActivationTimeCount = 0.0f;

    /// <summary> �K�E�Z�|�C���g���܂芄��(0.0~1.0) </summary>
    protected ReactiveProperty<float> m_spMovePointRate = new ReactiveProperty<float>(0.0f);

    /// <summary> �v���C���[�|�C���g�R���|�[�l���g </summary>
    protected PlayerPoint_CannonFight m_playerPoint;

    /// <summary> �v���C���[����R���|�[�l���g </summary>
    protected PlayerController_CannonFight m_playerController;

    /// <summary> �v���C���[����R���|�[�l���g </summary>
    protected PlayerImpact_CannonFight m_playerImpact;

    #endregion

    #region �Q�b�^�[

    public IReadOnlyReactiveProperty<float> m_SpMovePointRate => m_spMovePointRate;

    public bool m_IsSpMovePointMaxCharge { get { return m_isSpMovePointMaxCharge; } }

    public bool m_IsSpMove { get { return m_isSpMove; } }

    public bool m_IsChangeDuringSpMove_KickStickOutSpeed { get { return m_isChangeDuringSpMove_KickStickOutSpeed; } }

    public float m_DuringSpMove_KickStickOutSpeed { get { return m_duringSpMove_KickStickOutSpeed; } }

    public bool m_IsChangeDuringSpMove_KickRetractSpeed { get { return m_isChangeDuringSpMove_KickRetractSpeed; } }

    public float m_DuringSpMove_KickRetractSpeed { get { return m_duringSpMove_KickRetractSpeed; } }

    public bool m_IsChangeDuringSpMove_KickPlatformPower { get { return m_isChangeDuringSpMove_KickPlatformPower; } }

    public float m_DuringSpMove_KickPlatformPower { get { return m_duringSpMove_KickPlatformPower; } }

    public bool m_IsChangeDuringSpMove_KickPlayerPower { get { return m_isChangeDuringSpMove_KickPlayerPower; } }

    public float m_DuringSpMove_KickPlayerPower { get { return m_duringSpMove_KickPlayerPower; } }

    public bool m_IsChangeDuringSpMove_IsNotPassiveKick { get { return m_isChangeDuringSpMove_IsNotPassiveKick; } }

    public bool m_DuringSpMove_IsNotPassiveKick { get { return m_duringSpMove_IsNotPassiveKick; } }

    public bool m_IsChangeDuringSpMove_NotPassiveTime { get { return m_isChangeDuringSpMove_NotPassiveTime; } }

    public float m_DuringSpMove_NotPassiveTime { get { return m_duringSpMove_NotPassiveTime; } }

    #endregion


    protected virtual void Start()
    {
        m_playerPoint = GetComponent<PlayerPoint_CannonFight>();
        m_playerController = GetComponent<PlayerController_CannonFight>();
        m_playerImpact = GetComponent<PlayerImpact_CannonFight>();
    }

    protected virtual void Update()
    {
        if (m_isSpMove)
        {
            // �K�E�Z�����c�莞�Ԃ̍X�V
            UpdateSpMoveTime();
        }
        else
        {
            if (m_playerController.m_State == PlayerController.State.Play)
            {
                // ���Ԍo�߂ɂ��K�E�Z�Q�[�W��������
                AccumulateTimePattern();
            }
        }
    }

    /// <summary> �R��ꂽ���ɂ��Q�[�W���� </summary>
    public void AccumulateBeKickedPattern()
    {
        if (!m_isUseBeKickedPattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_bekickedPatternRateOfUp * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_bekickedPatternRateOfUp;
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> �G�v���C���[���R�������ɂ��Q�[�W���� </summary>
    public void AccumulateKickPattern()
    {
        if (!m_isUseKickPattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_kickPatternRateOfUp * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_kickPatternRateOfUp;
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> �G�v���C���[�ɓ|���ꂽ���ɂ��Q�[�W���� </summary>
    public void AccumulateBeKilledPattern()
    {
        if (!m_isUseBeKilledPattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_bekilledPatternRateOfUp * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_bekilledPatternRateOfUp;
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> ���Ԍo�߂ɂ��Q�[�W���� </summary>
    private void AccumulateTimePattern()
    {
        if (!m_isUseTimePattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_timePatternRateOfUp * (Time.deltaTime / 0.1f) * m_timePatternRankMag[m_playerPoint.m_Rank - 1] * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_timePatternRateOfUp * (Time.deltaTime / 0.1f) * m_timePatternRankMag[m_playerPoint.m_Rank - 1];
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> �K�E�Z�|�C���g�����܂������ǂ����m�F���� </summary>
    private void CheckSpMovePointMaxCharge()
    {
        // �K�E�Z�|�C���g�����܂肫���Ă��邩
        if (m_spMovePoint >= m_spMovePointMax)
        {
            m_spMovePoint = m_spMovePointMax;
            m_isSpMovePointMaxCharge = true;
            m_isCanSpMoveEffect.SetActive(true);
            m_isCanSpMoveEffect.GetComponent<Animator>().SetBool("IsShow", true);
            m_spmoveChargeEffect.GetChild(0).GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySe(
                "�L���m���t�@�C�g_�K�E�Q�[�W���^����",
                false);
            // �R���g���[���[�U��
            VibrationManager.Instance.SetVibration(GetComponent<PlayerId>().m_Id, 40, 0.7f);
        }
    }

    /// <summary> �K�E�Z�𔭓����� </summary>
    public virtual void StartSpMove()
    {
        m_isSpMovePointMaxCharge = false;
        m_isCanSpMoveEffect.SetActive(false);
        m_isCanSpMoveEffect.GetComponent<Animator>().SetBool("IsShow", false);
        m_isSpMove = true;
        m_spMoveActivationTimeCount = m_spMoveActivationTime;
        // �K�E�Z�������p�p�����[�^�𔽉f
        m_playerController.SetDuringSpMoveParam();
        m_playerImpact.SetDuringSpMoveParam();
    }

    /// <summary> �K�E�Z�̎c�莞�Ԃ��X�V </summary>
    private void UpdateSpMoveTime()
    {
        m_spMoveActivationTimeCount -= Time.deltaTime;

        // �K�E�Z�̎c�莞�Ԃ������Ȃ�����
        if (m_spMoveActivationTimeCount <= 0.0f)
        {
            m_spMovePointRate.Value = 0.0f;
            // �K�E�Z���I��
            EndSpMove();
            ResetSpMovePoint();
        }
        else
        {
            m_spMovePointRate.Value = m_spMoveActivationTimeCount / m_spMoveActivationTime;
        }
    }

    /// <summary> �K�E�Z���I������ </summary>
    public virtual void EndSpMove()
    {
        m_isSpMove = false;
        m_spMoveActivationTimeCount = 0.0f;
        // �K�E�Z�������p�p�����[�^�𔽉f
        m_playerController.SetDefaultParam();
        m_playerImpact.SetDefaultParam();
    }

    /// <summary> �K�E�Z�|�C���g�����Z�b�g���� </summary>
    public void ResetSpMovePoint()
    {
        m_spMovePoint = 0.0f;
        m_spMovePointRate.Value = 0.0f;
        m_isSpMovePointMaxCharge = false;
        m_isCanSpMoveEffect.SetActive(false);
        m_isCanSpMoveEffect.GetComponent<Animator>().SetBool("IsShow", false);
    }
}
