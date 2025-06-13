/*******************************************************************************
*
*	�^�C�g���F	���e�̃f�[�^�@�C���^�[�t�F�[�X�ɕύX���邩���H�H�H
*	�t�@�C���F	BombCharacter.cs
*	�쐬�ҁF	�� �喲
*	������F    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bomb_CountDown : MonoBehaviour, IBomb
{
    #region �t�B�[���h�p�����[�^

    [SerializeField, CustomLabel("�X�|�[�����̔��ˈЗ�")]
    float m_spawPower;

    [SerializeField, CustomLabel("���e�̎c�莞��(�b)")]
    float m_aliveTime;

    [SerializeField, CustomLabel("���e�̈З�")]
    float m_bombDamage;

    /// <summary> ���e�����j���������Ă��Ȃ��� </summary>
    bool m_isExprosition = false;

    BombCharacter m_bombCharacter;

    CinemachineImpulseSource m_ImpulseSource;

    [SerializeField]
    string m_exprosionEffectName = "���e����_��";

    [SerializeField]
    string m_exprosionSoundName = "���e����_��";

    [SerializeField, CustomLabel("�����̐U������")]
    private int m_vibrationFrame;

    [SerializeField, CustomLabel("�����̐U���̋���")]
    private float m_vibrationPower;

    #endregion

    #region �v���p�e�B


    /// <summary> ���e����ǂ̂��炢�Ŕ�������̂� </summary>
    /// <returns> ���e�̎c�莞��(�b) </returns>
    public float GetAliveTime() { return m_aliveTime; }

    /// <summary> ���e���������Ă���̂��ǂ��� </summary>
    /// <returns> ���e�����j���������Ă��Ȃ��� </returns>
    public bool GetisExprosition() { return m_isExprosition; }

    /// <summary> �����ŃG���A������З� </summary>
    /// <returns> ���e�̈З� </returns>
    public float GetBombDamage() { return m_bombDamage; }


    /// <summary> �������g�̃Q�[���I�u�W�F�N�g </summary>
    /// <returns> GameObject </returns>
    public GameObject GetGameObject() { return gameObject; }


    #endregion

    public void StartImpact(Vector3 _target)
    {
        GetComponent<Rigidbody2D>().velocity = _target * m_spawPower;
        //GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(_rotation), Mathf.Sin(_rotation), 0.0f) * m_spawPower;
        //Debug.Log("Cos" + Mathf.Cos(_rotation));
        //Debug.Log("Sin" + Mathf.Sin(_rotation));
    }

    private void Start()
    {
        m_bombCharacter = GetComponent<BombCharacter>();

        m_ImpulseSource = GetComponent<CinemachineImpulseSource>();

        float timer = Timer.Instance.m_TimeCounter;
        float bombSpeed = BombManager.Instance.BombMultSpeed;

        // �Q�[���I�����ԁ[�������Ԃ����Ȃ��Ȃ肷���Ȃ��悤�ɂ���
        float amountTime = 13.0f;
        if (timer - m_aliveTime <= amountTime)    // amountTime�b�ȏ㌸�炳�Ȃ�
        {
            m_aliveTime = amountTime * bombSpeed;
        }


        // ���̔��e�����������ꍇ�A�c�莞�Ԃ�1�b�Ƃ��Z�����Ă��܂��ꍇ�A���e�̎c�莞�Ԃ𑝂₷
        float endSpawTime = 20 * bombSpeed;         // �Ō�ɃX�|�[����������E����
        if ((timer * bombSpeed)  <= endSpawTime)   // endSpawTime���A���Ԃ̕���������������A���ԕ����e�̎c�莞�Ԃ𑝂₷
        {
            m_aliveTime = timer * bombSpeed - 0.1f;
        }


        //// �������Ԃ��Q�[���I�����Ԃɍ��킹��
        //if ((timer * bombSpeed) < m_aliveTime)
        //{
        //    m_aliveTime = (timer * bombSpeed) - 0.1f;
        //}

    }

    private void Update()
    {
        if (m_isExprosition)
        {
            return;
        }

        // �Q�[���̎��Ԃ��I����Ă�����
        if (Timer.Instance.m_TimeCounter <= 0.01f)
        {
            return;
        }


        m_aliveTime -= Time.deltaTime * BombManager.Instance.BombMultSpeed;

        if (m_aliveTime < 0.0f)
        {
            m_aliveTime = 0.0f;

            // ���������u��
            if (!m_isExprosition)
            {
                transform.GetChild(0).gameObject.SetActive(false);

                m_isExprosition = true;

            }
        }


        if (m_isExprosition)
        {
            // �G�t�F�N�g���o��
            EffectContainer.Instance.EffectPlay(m_exprosionEffectName, transform.position);

            // �T�E���h
            AudioManager.Instance.PlaySe(m_exprosionSoundName, false);

            // �R���g���[���[�U��
            VibrationManager.Instance.SetVibration(m_bombCharacter.m_InAreaNumber, m_vibrationFrame, m_vibrationPower);

            // �G���A��HP�����炷
            BombGame_PlayAreaHealthManager.Instance.SubHealth(m_bombCharacter.m_InAreaNumber, m_bombDamage);

            //// ���̔��e���X�|�[��
            //BombManager.Instance.m_spawFlag = true;

            // ���e���X�g�̗v�f���폜
            BombManager.Instance.RemoveBombList(m_bombCharacter);

            // ���e�̐U�����J�����ɓ`����
            m_ImpulseSource.GenerateImpulse();

            //// �I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
    }


}
