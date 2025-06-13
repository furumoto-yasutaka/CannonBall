using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CanvasGroupAlpha : MonoBehaviour
{
    public enum STATE
    {
        NONE,
        IN,
        OUT,
    }

    [SerializeField]
    float m_InSpeed = 1.0f;

    [SerializeField]
    float m_OutSpeed = 1.0f;


    CanvasGroup m_canvasGroup;
    STATE m_state;

    public void AlphaIn() { m_state = STATE.IN; }
    public void AlphaOut() { m_state = STATE.OUT; }

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {

        switch (m_state)
        {
            case STATE.NONE:
                break;
            case STATE.IN:
                m_canvasGroup.alpha += m_InSpeed * Time.deltaTime;
                if (m_canvasGroup.alpha >= 1.0f)
                {
                    m_canvasGroup.alpha = 1.0f;
                    m_state = STATE.NONE;
                }
                break;
            case STATE.OUT:
                m_canvasGroup.alpha -= m_OutSpeed * Time.deltaTime;
                if (m_canvasGroup.alpha <= 0.0f)
                {
                    m_canvasGroup.alpha = 0.0f;
                    m_state = STATE.NONE;
                }
                break;
        }
    }
}
