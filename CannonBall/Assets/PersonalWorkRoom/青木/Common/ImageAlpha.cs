using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlpha : MonoBehaviour
{
    [SerializeField]
    float m_speed = 1.0f;

    [SerializeField]
    float m_lowerAlpha = 0.0f;

    Color m_color;

    float m_timeCount = 0.0f;

    Image m_image;

    private void Start()
    {
        m_image = GetComponent<Image>();
        m_color = m_image.color;
    }

    private void Update()
    {
        m_timeCount += m_speed * Time.deltaTime;

        m_color.a = m_lowerAlpha + Mathf.PingPong(m_timeCount, 1.0f - m_lowerAlpha);

        m_image.color = m_color;
    }

}
