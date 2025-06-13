using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuideView : MonoBehaviour
{

    bool m_isHide = false;


    bool m_isLogoDisable = false;
    bool m_isSlideDisable = false;

    Animator m_animator;
    RectTransform m_rectTransform;
    Vector3 m_defaultPosition;

    public bool GetHide() { return m_isHide; }


    public void SetHide() { m_isHide = true; }
    public void SetLogoDesable() { m_isLogoDisable = true; }
    public void SetSlideDisable() { m_isSlideDisable = true; }


    private void OnEnable()
    {
        m_isLogoDisable = false;
        m_isSlideDisable = false;
    }


    private void OnDisable()
    {
        if (m_rectTransform != null)
        {
            m_rectTransform.localPosition = m_defaultPosition;
        }
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_rectTransform = GetComponent<RectTransform>();
        m_defaultPosition = m_rectTransform.localPosition;
    }



    private void Update()
    {
        if (m_isLogoDisable && m_isSlideDisable)
        {
            m_isHide = false;

            m_isLogoDisable = false;
            m_isSlideDisable = false;

            gameObject.SetActive(false);
        }
    }


}
