using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationsWait : MonoBehaviour
{
    [SerializeField]
    float m_waitTime = 0.0f;

    float m_baseTime = 0.0f;

    float m_timeCount = 0;

    bool m_isTimeCount = false;

    bool m_isAnimationStart = false;


    public void SetIsTimeCount() { m_isTimeCount = true; }
    public void SetBaseTime(float _time) {  m_baseTime = _time; }
    public bool GetIsAnimationStart() { return m_isAnimationStart; }
    public float GetWaitTime() { return m_waitTime; }

    private void Update()
    {
        if (m_isTimeCount)
        {
            m_timeCount += Time.deltaTime;

            if (m_baseTime + m_waitTime <= m_timeCount)
            {
                m_isAnimationStart = true;

                enabled = false;
            }
        }
    }


}
