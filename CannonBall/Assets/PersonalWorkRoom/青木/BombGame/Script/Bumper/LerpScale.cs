using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScale : MonoBehaviour
{

    [SerializeField]
    float m_maxScale;

    [SerializeField]
    float m_exTime;

    float m_countTime;


    Vector3 m_defaultScale;


    private void Start()
    {
        m_defaultScale = transform.localScale;
    }


    private void FixedUpdate()
    {
        m_countTime += Time.deltaTime;

        if (m_countTime >= m_exTime)
        {
            m_countTime = 0.0f;

            transform.localScale = m_defaultScale;
        }
        else
        {
            transform.localScale = Mathf.Lerp(m_defaultScale.x, m_maxScale, m_countTime / m_exTime) * Vector3.one;
        }

    }


}
