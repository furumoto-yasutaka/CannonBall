using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyOnCollision_BombRush : PlayerBodyOnCollision
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.transform.CompareTag("Bomb"))
        {
            OnCollisionEnter_Bomb(collision);
        }
    }

    private void OnCollisionEnter_Bomb(Collision2D collision)
    {
        if (m_playerController.m_IsHeadbutt)
        {
            Vector3 normalDist = collision.transform.position - transform.root.position;
            float dot = Vector2.Dot(m_rb.velocity.normalized, normalDist.normalized);
            if (dot < 0.0f)
            {
                collision.transform.GetComponent<BombImpact>().Headbutted(
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
            "プレイヤー_バウンド",
            false,
            Mathf.Clamp(sqrMag / m_playerImpactEffectInfoMap.m_Threshold[0], 0.0f, m_bounceSeVolumeMax));
    }
}
