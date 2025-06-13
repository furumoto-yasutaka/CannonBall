using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerBumper_BombRush : CornerBumper
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            collision.transform.root.GetComponent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

            m_animator.SetTrigger("IsBounce");
            CreateContactEffect();
        }
        else if (collision.CompareTag("PlayerBody"))
        {
            Transform root = collision.transform.root;
            if (!root.GetComponent<PlayerController>().m_BumperIgnore)
            {
                root.GetComponentInParent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

                m_animator.SetTrigger("IsBounce");
                CreateContactEffect();
            }
        }
    }
}
