/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̑��̓����蔻��̃C�x���g�X�N���v�g
*	�t�@�C���F	PlayerLegOnCollision.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/12
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision : MonoBehaviour
{
    /// <summary> ���W�b�h�{�f�B </summary>
    protected Rigidbody2D m_rb;

    /// <summary> �v���C���[���͎�t </summary>
    protected PlayerController m_playerController;

    /// <summary> �v���C���[���͎�t </summary>
    protected PlayerImpact m_playerImpact;

    /// <summary> ���݂̃t���[���Œn�`���R������ </summary>
    protected bool m_isKickPlatform = false;

    /// <summary> �R�蒆�ɓ��������v���C���[ </summary>
    protected List<GameObject> m_contactList = new List<GameObject>();


    protected virtual void Start()
    {
        Transform p = transform.root;
        m_rb = p.GetComponent<Rigidbody2D>();
        m_playerController = p.GetComponent<PlayerController>();
        m_playerImpact = p.GetComponent<PlayerImpact>();
    }

    protected void Update()
    {
        m_isKickPlatform = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBody"))
        {// �G�v���C���[���R����
            OnTriggerEnter_Player(other);
        }
        else if (other.CompareTag("Platform"))
        {// �n�`���R����
            OnTriggerEnter_Platform(other);
        }
    }

    protected virtual void OnTriggerEnter_Player(Collider2D other)
    {
        m_playerImpact.KickPlayerRecoil();
        other.transform.root.GetComponent<PlayerImpact>().Kicked(
            m_playerController.m_KickDir,
            m_playerImpact.m_KickPlayerPower,
            m_rb.velocity);

        // ����̏R��ŏ��߂ē��������I�u�W�F�N�g�̏ꍇ�̓G�t�F�N�g���o��
        bool find = false;
        foreach (GameObject obj in m_contactList)
        {
            if (obj == other.gameObject)
            {
                find = true;
            }
        }
        if (!find)
        {
            m_contactList.Add(other.gameObject);
            int id = transform.root.GetComponent<PlayerId>().m_Id + 1;
            string type = PlayerController.m_TypeStr[(int)m_playerController.m_Type];
            Vector3 offset = m_playerController.m_KickDir * 0.5f;
            EffectContainer.Instance.EffectPlay(
                "�z���O�����q�b�g���G�t�F�N�g_" + id + "P_" + type,
                other.ClosestPoint(transform.position + offset));
            AudioManager.Instance.PlaySe(
                "�v���C���[_�q�b�g��(�v���C���[��)_" + PlayerController.m_TypeStr[(int)m_playerController.m_Type],
                false);
            VibrationManager.Instance.SetVibration(id - 1, 8, 0.6f);
        }

        // �󂯐g�s��ԂɂȂ�R�肾������
        if (m_playerImpact.m_IsNotPassiveKick)
        {
            // �R��ꂽ������󂯐g�s��Ԃɂ���
            other.transform.root.GetComponent<PlayerImpact>().SetNotPassive(m_playerImpact.m_NotPassiveTime);
        }
    }

    protected virtual void OnTriggerEnter_Platform(Collider2D other)
    {
        if (m_isKickPlatform) { return; }

        bool find = false;
        foreach (GameObject obj in m_contactList)
        {
            if (obj == other.gameObject)
            {
                find = true;
            }
        }
        if (!find)
        {
            m_contactList.Add(other.gameObject);
            m_isKickPlatform = true;
            m_playerImpact.KickPlatform();
            m_playerController.StartHeadbutt();
            int id = transform.root.GetComponent<PlayerId>().m_Id + 1;
            Vector3 offset = m_playerController.m_KickDir * 0.5f;
            EffectContainer.Instance.EffectPlay(
                "�n�`�R�莞_" + id + "P",
                other.ClosestPoint(transform.position + offset));
            AudioManager.Instance.PlaySe(
                "�v���C���[_�q�b�g��(�n�`)",
                false);
            VibrationManager.Instance.SetVibration(id - 1, 8, 0.3f);
        }
    }

    public void ResetContactList()
    {
        m_contactList.Clear();
    }
}
