using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Health : MonoBehaviour, IBomb
{
    #region �t�B�[���h�p�����[�^

    [SerializeField, CustomLabel("�X�|�[�����̔��ˈЗ�")]
    float m_spawPower;

    [SerializeField, CustomLabel("���e�̎c�莞��(�b)")]
    float m_aliveTime;

    [SerializeField, CustomLabel("���e(��)�̈З�")]
    float m_bombHealth;

    /// <summary> ���e�����j���������Ă��Ȃ��� </summary>
    bool m_isExprosition = false;

    BombCharacter m_bombCharacter;

    CinemachineImpulseSource m_ImpulseSource;

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
    public float GetBombDamage() { return m_bombHealth; }


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
    }

    private void Update()
    {
        // ���łɃV�[�P���X�ɔ������Ă�����
        if (m_isExprosition)
        {
            return;
        }
        // �Q�[���̎��Ԃ��I����Ă�����
        if (Timer.Instance.m_TimeCounter <= 0.1f)
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
            EffectContainer.Instance.EffectPlay("�񕜔���", transform.position);

            // �G���A��HP�����炷
            BombGame_PlayAreaHealthManager.Instance.AddHealth(m_bombCharacter.m_InAreaNumber, m_bombHealth);

            // ���e���X�g�̗v�f���폜
            BombManager.Instance.RemoveBombList(m_bombCharacter);

            // ���e�̐U�����J�����ɓ`����
            m_ImpulseSource.GenerateImpulse();

            //// �I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
    }





}
