using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWallHitSound : MonoBehaviour
{
    Rigidbody2D m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            float vel = (m_rb.velocity.x + m_rb.velocity.y ) * 0.5f;

            if (vel >= 0.1f)
            {
                AudioManager.Instance.PlaySe("���e���ǂɐG��鉹_1", false);
                AudioManager.Instance.PlaySe("���e���ǂɐG��鉹_2", false);
            }
        }
    }
}
