using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLastColor : MonoBehaviour
{
    [SerializeField]
    Color m_StartColor;

    [SerializeField]
    Color m_EndColor;


    [SerializeField]
    MeshRenderer[] m_MeshRenderers;


    IBomb m_bomb;

    float m_T;


    Color m_Color;


    private void Start()
    {
        m_bomb = GetComponent<IBomb>();
    }

    private void Update()
    {
        // ƒQ[ƒ€‚ÌŠÔ‚ªI‚í‚Á‚Ä‚¢‚½‚ç
        if (Timer.Instance.m_TimeCounter <= 0.1f)
        {
            return;
        }


        if (m_bomb.GetAliveTime() <= 6.0f)
        {
            m_T += Time.deltaTime * BombManager.Instance.BombMultSpeed;


            m_Color = Color.Lerp(m_StartColor, m_EndColor, Mathf.Repeat(m_T, 1.0f));


            foreach (var item in m_MeshRenderers)
            {
                item.material.SetColor("_Color", m_Color);
            }
        }
    }



}
