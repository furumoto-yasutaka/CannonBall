using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class FadeDuringResult : MonoBehaviour
{
    enum STATE
    {
        NONE,
        FADE_OUT,
        WAIT,
        FADE_IN,
    }

    [SerializeField]
    float m_speed = 1.0f;

    [SerializeField]
    float m_waitTime = 0.75f;
    float m_timeCount = 0.0f;

    Image m_image;

    Color m_color;
    float m_alpha = 0f;

    STATE m_state = STATE.NONE;

    public void SetFadeOutStart() { m_state = STATE.FADE_OUT; }

    private void Start()
    {
        m_image = GetComponent<Image>();
        m_color = m_image.color;

    }

    private void Update()
    {
        switch (m_state)
        {
            case STATE.NONE:
                break;
            case STATE.FADE_OUT:
                FadeOut();
                break;
            case STATE.WAIT:
                Wait();
                break;
            case STATE.FADE_IN:
                FadeIn();
                break;
            default:
                break;
        }
    }


    private void FadeOut()
    {
        m_alpha += m_speed * Time.deltaTime;

        if (m_alpha >= 1.0f)
        {
            ResultSceneController.Instance.SetState(ResultSceneController.STATE.SHOW_WINNER);

            m_alpha = 1.0f;

            m_state = STATE.WAIT;
        }

        m_image.color = new Color(m_color.r, m_color.g, m_color.b, m_alpha);
    }

    private void Wait()
    {
        m_timeCount += Time.deltaTime;

        if (m_timeCount >= m_waitTime)
        {
            m_state = STATE.FADE_IN;
        }
    }

    private void FadeIn()
    {
        m_alpha -= m_speed * Time.deltaTime;

        if (m_alpha <= 0.0f)
        {
            m_alpha = 0.0f;

            m_state = STATE.NONE;
        }
        m_image.color = new Color(m_color.r, m_color.g, m_color.b, m_alpha);

    }
}
