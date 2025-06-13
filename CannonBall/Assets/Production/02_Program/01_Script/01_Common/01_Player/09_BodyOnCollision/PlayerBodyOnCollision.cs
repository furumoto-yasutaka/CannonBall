/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̑̂̃R���W�����C�x���g�X�N���v�g
*	�t�@�C���F	PlayerBodyOnCollision.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyOnCollision : MonoBehaviour
{
    /// <summary> ���W�b�h�{�f�B�R���|�[�l���g </summary>
    [SerializeField, CustomLabel("���W�b�h�{�f�B�R���|�[�l���g")]
    protected Rigidbody2D m_rb;

    [SerializeField, CustomLabel("�n�`�Ƃ̏Ռ��G�t�F�N�g���")]
    private BodyCollisionEffectInfoMap m_platformImpactEffectInfoMap;

    [SerializeField, CustomLabel("�v���C���[�Ƃ̏Ռ��G�t�F�N�g���")]
    protected BodyCollisionEffectInfoMap m_playerImpactEffectInfoMap;

    [SerializeField, CustomLabel("�n�`�Փˎ���SE�̍ő剹��")]
    protected float m_bounceSeVolumeMax = 1.0f;

    protected PlayerController m_playerController;

    protected PlayerImpact m_playerImpact;

    protected bool m_isPlayBoundSe = true;


    protected virtual void Start()
    {
        Transform root = transform.root;
        m_playerController = root.GetComponent<PlayerController>();
        m_playerImpact = root.GetComponent<PlayerImpact>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnCollisionEnter_Player(collision);
        }
        else if (collision.transform.CompareTag("Platform"))
        {
            OnCollisionEnter_Platform(collision);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision) {}

    /// <summary> �v���C���[���G�ꂽ�ۂ̃C�x���g </summary>
    /// <param name="collision"> �v���C���[�̃R���W���� </param>
    protected virtual void OnCollisionEnter_Player(Collision2D collision)
    {
        if (m_playerController.m_IsHeadbutt)
        {
            Vector3 normalDist = collision.transform.position - transform.root.position;
            float dot = Vector2.Dot(m_rb.velocity.normalized, normalDist.normalized);
            if (dot < 0.0f)
            {
                collision.transform.GetComponent<PlayerImpact>().Headbutted(
                    normalDist.normalized,
                    m_playerImpact.m_HeadbuttBasePower,
                    m_rb.velocity);
            }
        }

        float sqrMag = m_rb.velocity.sqrMagnitude;
        for (int i = 0; i < m_playerImpactEffectInfoMap.m_Threshold.Length; i++)
        {
            float threshold = m_playerImpactEffectInfoMap.m_Threshold[i] * m_playerImpactEffectInfoMap.m_Threshold[i];
            if (sqrMag > threshold)
            {
                EffectContainer.Instance.EffectPlay(
                    m_playerImpactEffectInfoMap.m_EffectName[i],
                    collision.collider.ClosestPoint(transform.position));
                break;
            }
        }

        AudioManager.Instance.PlaySe(
            "�v���C���[_�o�E���h",
            false,
            Mathf.Clamp(sqrMag / m_playerImpactEffectInfoMap.m_Threshold[0], 0.0f, m_bounceSeVolumeMax));
    }

    /// <summary> �n�`���G�ꂽ�ۂ̃C�x���g </summary>
    /// <param name="collision"> �n�`�̃R���W���� </param>
    protected virtual void OnCollisionEnter_Platform(Collision2D collision)
    {
        float sqrMag = m_rb.velocity.sqrMagnitude;
        for (int i = 0; i < m_platformImpactEffectInfoMap.m_Threshold.Length; i++)
        {
            float threshold = m_platformImpactEffectInfoMap.m_Threshold[i] * m_platformImpactEffectInfoMap.m_Threshold[i];
            if (sqrMag > threshold)
            {
                EffectContainer.Instance.EffectPlay(
                    m_platformImpactEffectInfoMap.m_EffectName[i] + "_" + (transform.root.GetComponent<PlayerId>().m_Id + 1) + "P",
                    collision.collider.ClosestPoint(transform.position));
                break;
            }
        }

        if (m_isPlayBoundSe)
        {
            AudioManager.Instance.PlaySe(
                "�v���C���[_�o�E���h",
                false,
                Mathf.Clamp(sqrMag / m_platformImpactEffectInfoMap.m_Threshold[0], 0.0f, m_bounceSeVolumeMax));
        }
    }

    public void SetPlayBounceSe(bool flag)
    {
        m_isPlayBoundSe = flag;
    }
}
