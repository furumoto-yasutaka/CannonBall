using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationsManager : MonoBehaviour
{
    [SerializeField]
    public bool m_IsStart = false;

    [SerializeField]
    Animator[] m_animators;


    CongratulationsWait[] m_congratulationsWait;


    float m_timeCount = 0.0f;

    int m_currentCharIndex = 0;


    private void Start()
    {
        m_animators = new Animator[transform.childCount];
        m_congratulationsWait = new CongratulationsWait[transform.childCount];

        float baseTime = 0.0f;
        for (int i = 0; i < transform.childCount; i++) 
        {
            m_animators[i] = transform.GetChild(i).GetComponent<Animator>();
            m_congratulationsWait[i] = transform.GetChild(i).GetComponent<CongratulationsWait>();

            m_congratulationsWait[i].SetBaseTime(baseTime);

            baseTime += m_congratulationsWait[i].GetWaitTime();


        }
    }

    private void Update()
    {
        if (m_IsStart)
        {
            m_timeCount += Time.deltaTime;

            if (m_congratulationsWait[m_currentCharIndex].GetIsAnimationStart())
            {
                m_animators[m_currentCharIndex].SetTrigger("Start");

                m_currentCharIndex++;

                m_currentCharIndex = Mathf.Clamp(m_currentCharIndex, 0, transform.childCount - 1);
            }

            // m_isStart==trueÇ…Ç»Ç¡ÇΩÇÁàÍâÒÇæÇØèàóùÇ≥ÇπÇÍÇŒÇ¢Ç¢
            for (int i = 0; i < transform.childCount; i++)
            {
                m_congratulationsWait[i].SetIsTimeCount();
            }
        }
    }




    public void CongratulationsStart()
    {
        m_IsStart = true;
    }


}
