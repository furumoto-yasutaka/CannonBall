using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision_TutorialTargetKick : PlayerLegOnCollision
{
    [SerializeField]
    float m_power = 1f;

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
            // プレイヤーにノックバック
            m_playerImpact.KickPlayerRecoil();

            // ボムを飛ばす
            Rigidbody2D rb = other.transform.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.AddForce(m_playerController.m_KickDir * m_power);
            }
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
                "ホログラムヒット時エフェクト_" + id + "P_" + type,
                other.ClosestPoint(transform.position + offset));
            AudioManager.Instance.PlaySe(
                "プレイヤー_ヒット音(プレイヤー等)_" + PlayerController.m_TypeStr[(int)m_playerController.m_Type],
                false);
        }
    }
}
