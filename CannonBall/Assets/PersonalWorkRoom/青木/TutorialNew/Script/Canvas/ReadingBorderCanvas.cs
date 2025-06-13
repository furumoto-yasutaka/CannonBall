using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ReadingBorderCanvas : MonoBehaviour
{
    enum STATE
    {
        NONE,
        IN,
        OUT,
    }


    [SerializeField]
    float m_speed = 2.0f;

    [SerializeField]
    float m_delay = 0.0f;

    STATE m_state;

    RectTransform[] m_borders = new RectTransform[2];

    #region ì‡ïîä÷êî

    RectTransform m_rectTransform;
    float m_height = 0.0f;

    float m_moveLength = 0.0f;


    float m_targetIn = 0.0f;
    float m_targetOut = 0.0f;

    #endregion


    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();

        m_moveLength = m_height = m_rectTransform.rect.height;
        m_targetIn = 1080.0f * 0.5f - m_moveLength;
        m_targetOut = 1080.0f * 0.5f + m_moveLength;

        for (int i = 0; i < m_borders.Length; i++)
        {
            m_borders[i] = transform.GetChild(i).GetComponent<RectTransform>();
        } 

    }

    private void Update()
    {
        switch (m_state)
        {
            case STATE.NONE:
                break;
            case STATE.IN:
                m_borders[0].position -= m_speed * Time.deltaTime * Vector3.up;
                m_borders[1].position += m_speed * Time.deltaTime * Vector3.up;

                if (m_borders[0].position.y <= m_targetIn)
                {
                    m_state = STATE.NONE;
                    m_borders[0].position = m_targetIn * Vector3.up;
                    m_borders[1].position = -m_targetIn * Vector3.up;
                }
                break;
            case STATE.OUT:
                m_borders[0].position += m_speed * Time.deltaTime * Vector3.up;
                m_borders[1].position -= m_speed * Time.deltaTime * Vector3.up;
                if (m_borders[0].position.y >= m_targetOut)
                {
                    m_state = STATE.NONE;
                    m_borders[0].position = m_targetOut * Vector3.up;
                    m_borders[1].position = -m_targetOut * Vector3.up;
                }
                break;
        }
    }



    public async void MoveDelayIn()
    {
        await Task.Delay((int)(m_delay * 1000));
        m_state = STATE.IN;
    }
    public async void MoveDelayOut()
    {
        await Task.Delay((int)(m_delay * 1000));
        m_state = STATE.OUT;
    }

}
