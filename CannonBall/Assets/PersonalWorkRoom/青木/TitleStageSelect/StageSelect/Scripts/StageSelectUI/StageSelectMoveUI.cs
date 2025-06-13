using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectMoveUI : MonoBehaviour
{
    enum State
    {
        NONE,
        IN,
        OUT,

    }


    [SerializeField]
    float m_speed = 0.25f;

    [SerializeField]
    float m_FlashingSpeed = 0.3f;

    [SerializeField]
    float m_lowerAlpha = 0.5f;

    Image m_image;

    float m_alpha = 0.0f;

    State m_state = State.NONE;

    float m_pingPongCount = 0.0f;

    private void Start()
    {
        m_image = GetComponent<Image>();


        StageChangeEventTiming.Instance.m_isMove.Subscribe(v =>
            {
                m_state = (!v) ? State.IN : State.OUT;

                m_alpha = Mathf.Clamp01(m_alpha);
            }
        ).AddTo(this);
    }

    private void Update()
    {
        switch (m_state)
        {
            case State.NONE:
                Flashing();
                break;
            case State.IN:
                ImageAlphaAdd();
                break;
            case State.OUT:
                ImageAlphaSub();
                break;
            default:
                break;
        }

        m_image.color = Color.white - (Color.black - Color.black * m_alpha);
    }


    private void Flashing()
    {
        m_pingPongCount += m_FlashingSpeed * Time.deltaTime;

        m_alpha = m_lowerAlpha + Mathf.PingPong(m_pingPongCount, 1.0f - m_lowerAlpha);
    }

    private void ImageAlphaAdd()
    {
        m_alpha += m_speed * Time.deltaTime;

        if (m_alpha >= 1.0f)
        {
            m_alpha = 1.0f;
            m_state = State.NONE;
            PingPongInit();
        }
    }


    private void ImageAlphaSub()
    {
        m_alpha -= m_speed * Time.deltaTime;

        if (m_alpha <= 0.0f)
        {
            m_alpha = 0.0f;
            m_state = State.NONE;
            PingPongInit();
        }
    }


    private void PingPongInit()
    {
        m_pingPongCount = m_alpha;
    }

}
