using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayGuideViewSlide : MonoBehaviour
{
    [SerializeField, CustomLabel("îwåi")]
    Image m_backGround;

    [SerializeField, CustomLabel("îwåiÉtÉåÅ[ÉÄ")]
    Image m_frameBackGround;

    [SerializeField, CustomLabel("ÉvÉåÉCìÆâÊ")]
    RawImage m_playDemo;

    [SerializeField, CustomLabel("ÉãÅ[Éãê‡ñæ")]
    TextMeshProUGUI m_ruleText;
    
    [SerializeField, CustomLabel("ëÄçÏï˚ñ@ê‡ñæ")]
    TextMeshProUGUI m_playText;

    [SerializeField, CustomLabel("ÇªÇÃëºÇÃâÊëú")]
    Image[] m_othersImage;

    PlayGuideView m_playGuideView;

    bool m_isHide = false;

    float m_alpha = 1.0f;
    readonly float m_speed = 3.5f;


    private void OnEnable()
    {
        m_alpha = 1.0f;
        m_isHide = false;

        SetColor();
    }


    private void Start()
    {
        m_playGuideView = transform.parent.GetComponent<PlayGuideView>();

        StageChangeEventTiming.Instance.m_isMove.Subscribe(v =>
        {
            //if (m_playGuideView.GetHide())
            //{
            //    m_isHide = true;
            //}

            if (v)
            {
                m_isHide = true;
            }
            else
            {
                Color color = Color.white;

                m_backGround.color = color;
                m_frameBackGround.color = color;
                m_playDemo.color = color;
                m_ruleText.color = color;
                m_playText.color = color;
                foreach (var item in m_othersImage)
                {
                    item.color = color;
                }
            }
        }
        ).AddTo(this);

    }



    private void Update()
    {
        if (m_isHide)
        {
            HideUpdate();
        }
    }




    void HideUpdate()
    {
        m_alpha -= m_speed * Time.deltaTime;

        SetColor();

        if (m_alpha <= 0.0f)
        {
            m_isHide = false;

            m_playGuideView.SetSlideDisable();
        }
    }


    void SetColor()
    {
        m_alpha = Mathf.Clamp01(m_alpha);


        Color color = Color.white - (Color.black - Color.black * m_alpha);

        m_backGround.color = color;
        m_frameBackGround.color = color;
        m_playDemo.color = color;
        m_ruleText.color = color;
        m_playText.color = color;

        foreach (var item in m_othersImage)
        {
            item.color = color;
        }
    }


}
