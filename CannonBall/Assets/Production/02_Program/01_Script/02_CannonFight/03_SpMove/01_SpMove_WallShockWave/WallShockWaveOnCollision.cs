using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShockWaveOnCollision : MonoBehaviour
{
    /// <summary> 衝撃波を生んだ本体 </summary>
    private GameObject m_origin;

    /// <summary> 衝撃波でぶっ飛ぶ強さ </summary>
    private float m_wallShockWavePower;

    /// <summary> 衝撃波の発生主と衝突したか </summary>
    private float m_collisionActivateTimeCount = 0.0f;


    private void Update()
    {
        // コリジョンの発生時間を更新
        if (m_collisionActivateTimeCount > 0.0f)
        {
            m_collisionActivateTimeCount -= Time.deltaTime;
            if (m_collisionActivateTimeCount <= 0.0f)
            {
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    public void InitParam(GameObject origin, float power, float collisionActivateTimeCount)
    {
        m_origin = origin;
        m_wallShockWavePower = power;
        m_collisionActivateTimeCount = collisionActivateTimeCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerBody"))
        {
            OnTriggerEnter_Player(collision);
        }
    }

    private void OnTriggerEnter_Player(Collider2D collision)
    {
        Transform root = collision.transform.root;
        if (root.gameObject == m_origin ||
            root.GetComponent<PlayerInvincible>().m_IsInvincible) { return; }

        root.GetComponent<PlayerPoint_CannonFight>().RequestKickMark(m_origin.GetComponent<PlayerPoint_CannonFight>());

        // ぶっ飛ばす方向を計算
        Vector2 dir = root.position - transform.position;
        dir.Normalize();

        root.GetComponent<Rigidbody2D>().velocity = dir * m_wallShockWavePower;
    }
}
