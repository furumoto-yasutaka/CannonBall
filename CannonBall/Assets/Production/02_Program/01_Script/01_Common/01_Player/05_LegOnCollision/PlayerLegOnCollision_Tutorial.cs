using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision_Tutorial : PlayerLegOnCollision
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Bomb"))
        {
            OnTriggerEnter_Block(other);
        }
    }

    private void OnTriggerEnter_Block(Collider2D other)
    {
        float sum = Mathf.Abs(m_playerController.m_KickDir.x) + Mathf.Abs(m_playerController.m_KickDir.y);

        if (sum >= 0.1f)
        {
            // �v���C���[�Ƀm�b�N�o�b�N
            m_playerImpact.KickPlayerRecoil();

            // �{�����΂�
            other.transform.GetComponent<BombImpact>().Impact(
                m_playerController.m_KickDir,
                m_playerImpact.m_KickPlayerPower,
                m_rb.velocity);
        }
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
    }
}
