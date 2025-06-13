using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStartScale : MonoBehaviour
{
    [SerializeField]
    float m_speed = 1.0f;


    [SerializeField]
    float m_startScale = 0.2f;

    float m_maxScale;

    float m_t = 0.0f;

    bool m_isStartOnce = false;


    float m_waitTime = 0.08f;
    float m_waitTimeCount = 0.0f;


    private void Start()
    {
        m_maxScale = transform.localScale.x;

    }

    private void Update()
    {
        if (!m_isStartOnce)
        {
            transform.localScale = m_startScale * Vector3.one;

            m_isStartOnce = true;
        }

        if (m_waitTime > m_waitTimeCount)
        {
            m_waitTimeCount += Time.deltaTime;

            return;
        }


        if (m_t >= 1.0f)
        {
            return;
        }

        m_t += m_speed * Time.deltaTime;

        transform.localScale = Mathf.Lerp(m_startScale, m_maxScale, m_t) * Vector3.one;
    }

}
