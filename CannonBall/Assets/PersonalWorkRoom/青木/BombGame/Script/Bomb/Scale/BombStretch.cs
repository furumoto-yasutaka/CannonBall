using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStretch : MonoBehaviour
{
    [SerializeField]
    AnimationCurve m_animCurve;

    [SerializeField]
    float m_UpScale = 0.2f;


    IBomb m_bomb;


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
            float f = m_bomb.GetAliveTime() % 1.0f;

            transform.localScale = ((m_animCurve.Evaluate(f) * m_UpScale) + 1.0f) * Vector3.one;
        }

    }
}
