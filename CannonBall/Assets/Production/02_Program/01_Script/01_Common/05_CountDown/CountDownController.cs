using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownController : MonoBehaviour
{
    private TextMeshProUGUI m_tmp;

    private Animator m_animator;

    private int m_count = 5;


    private void Start()
    {
        m_tmp = GetComponent<TextMeshProUGUI>();
        m_animator = GetComponent<Animator>();
        m_tmp.text = m_count.ToString();
    }

    private void Update()
    {
        if (!m_animator.enabled)
        {
            if (Timer.Instance.m_TimeCounter <= 5.0f)
            {
                m_animator.enabled = true;
            }
        }
    }

    public void SetNextNumber()
    {
        m_count--;
        m_tmp.text = m_count.ToString();
    }

    public void CheckEnd()
    {
        if (m_count == 1)
        {
            m_animator.SetBool("IsEnd", true);
        }
    }
}
