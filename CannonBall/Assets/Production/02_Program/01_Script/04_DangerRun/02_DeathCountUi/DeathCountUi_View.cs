using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeachCountUi_View : MonoBehaviour
{
    private int m_tempValue = 0;

    private Animator m_animator;

    private TextMeshProUGUI m_tmp;


    public void Init()
    {
        m_tmp = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        m_animator = GetComponent<Animator>();
    }

    public void SetValue(int value)
    {
        m_tempValue = value;
        m_animator.SetTrigger("Dec");
    }

    public void ChangeText()
    {
        m_tmp.text = m_tempValue.ToString();
    }
}
