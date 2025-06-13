using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerBumper_Circle : MonoBehaviour
{
    [SerializeField]
    protected float m_power;

    [SerializeField]
    protected float m_distanceTime;

    protected Rigidbody2D m_hitRb;
    protected bool m_hitting = false;
    protected bool m_impact = false;

    /// <summary> バンパー接触時のエフェクトの親オブジェクト </summary>
    [SerializeField, CustomLabel("バンパー接触時のエフェクトの親オブジェクト")]
    protected Transform m_contactEffParent;

    protected Animator m_animator;

    float m_countTime = 0.0f;


    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_hitting)
        {
            return;
        }


        m_countTime += Time.deltaTime;

        if (m_countTime >= m_distanceTime && m_hitting)
        {
            m_countTime = 0.0f;

            m_impact = true;
        }

        if (m_impact && m_hitting && m_hitRb != null)
        {
            Vector2 vec = new Vector2(m_hitRb.position.x - transform.position.x, m_hitRb.position.y - transform.position.y);
            m_hitRb.velocity = m_power * (m_hitRb.transform.position - transform.position).normalized;

            m_impact = false;
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            m_hitRb = collision.transform.root.GetComponent<Rigidbody2D>();

            m_hitting = true;

            m_impact = true;

            m_animator.SetTrigger("IsBounce");
            CreateContactEffect();
        }
        else if (collision.CompareTag("PlayerBody"))
        {
            if (!collision.transform.root.GetComponent<PlayerController>().m_BumperIgnore)
            {
                m_hitRb = collision.transform.root.GetComponentInParent<Rigidbody2D>();

                m_hitting = true;

                m_impact = true;

                m_animator.SetTrigger("IsBounce");
                CreateContactEffect();
            }
        }

    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            m_hitRb = null;

            m_hitting = false;
        }
        else if (collision.CompareTag("PlayerBody"))
        {
            if (!collision.transform.root.GetComponent<PlayerController>().m_BumperIgnore)
            {
                m_hitRb = null;

                m_hitting = false;
            }
        }
    }

    protected void CreateContactEffect()
    {
        EffectContainer.Instance.EffectPlay(
            "バンパー接触",
            transform.GetChild(0).GetChild(0).position,
            default,
            m_contactEffParent,
            transform.localScale);
    }
}
