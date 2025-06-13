using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class CommonSlideView : MonoBehaviour
{
    public enum STATE
    {
        NONE,
        VIEW,
        HIDE,
    }


    [SerializeField, CustomReadOnly]
    float m_delayTime = 0.2f;

    [SerializeField, CustomLabel("‰E–îˆó")]
    CommonSlideArrowView m_rightArrowView;

    [SerializeField, CustomLabel("¶–îˆó")]
    CommonSlideArrowView m_leftArrowView;


    [SerializeField]
    Image[] m_images;

    [SerializeField]
    TextMeshProUGUI[] m_texts;

    STATE m_state = STATE.NONE;

    float m_speed = 6.0f;

    float m_alpha = 0.0f;


    public STATE GetState() { return m_state; }

    private void Start()
    {
        StageChangeEventTiming.Instance.m_isMove.Subscribe(v =>
        {
            m_state = (!v) ? STATE.VIEW : STATE.HIDE;

            if (!v)
            {
                m_rightArrowView.SetState(CommonSlideArrowView.STATE.VIEW);
            }
            else 
            {
                m_rightArrowView.SetState(CommonSlideArrowView.STATE.HIDE);
                m_leftArrowView.SetState(CommonSlideArrowView.STATE.HIDE);
            }

            m_alpha = Mathf.Clamp01(m_alpha);
        }
        ).AddTo(this);

    }


    private void Update()
    {
        switch (m_state)
        {
            case STATE.VIEW:
                ImageAlphaAdd();
                break;
            case STATE.HIDE:
                ImageAlphaSub();
                break;
            default:
                break;
        }

    }


    async void ImageAlphaAdd()
    {
        await Task.Delay((int)(m_delayTime * 1000));

        m_alpha += m_speed * Time.deltaTime;

        ChangeAlpha();

        if (m_alpha >= 1.0f)
        {
            m_alpha = 1.0f;
            m_state = STATE.NONE;
        }
    }

    private void ImageAlphaSub()
    {
        m_alpha -= m_speed * Time.deltaTime;

        ChangeAlpha();

        if (m_alpha <= 0.0f)
        {
            m_alpha = 0.0f;
            m_state = STATE.NONE;
        }
    }


    private void ChangeAlpha()
    {
        Color color = Color.white - (Color.black - Color.black * m_alpha);

        foreach (var item in m_images)
        {
            item.color = color;
        }

        foreach (var item in m_texts)
        {
            item.color = color;
        }


    }

}
