using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerBumper_Tutorial : CornerBumper
{

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bomb"))
        {
            collision.transform.GetComponent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

            m_animator.SetTrigger("IsBounce");
            CreateContactEffect();
            PlayContactSound();
        }
        else if (collision.collider.CompareTag("PlayerBody"))
        {
            Transform root = collision.transform.root;
            if (!root.GetComponent<PlayerController>().m_BumperIgnore)
            {
                root.GetComponentInParent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

                m_animator.SetTrigger("IsBounce");
                CreateContactEffect();
                PlayContactSound();
                SetVibration(root.GetComponent<PlayerId>().m_Id);
            }
        }
        else if (collision.collider.CompareTag("PlayerLeg"))
        {
            Transform root = collision.transform.root;
            if (!root.GetComponent<PlayerController>().m_BumperIgnore)
            {
                root.GetComponentInParent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

                m_animator.SetTrigger("IsBounce");
                CreateContactEffect();
                PlayContactSound();
                SetVibration(root.GetComponent<PlayerId>().m_Id);
            }
        }
    }
}
