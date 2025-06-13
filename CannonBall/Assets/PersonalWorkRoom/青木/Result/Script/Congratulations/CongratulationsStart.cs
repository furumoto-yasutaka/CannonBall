using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationsStart : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera m_virtualCamera;
    CinemachineTrackedDolly m_trackedDolly;

    [SerializeField]
    float m_startPosition = 0.65f;

    CongratulationsManager m_manager;


    private void Start()
    {
        m_trackedDolly = m_virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        m_manager = GetComponent<CongratulationsManager>();
    }


    private void Update()
    {
        if (!m_manager.m_IsStart && m_trackedDolly.m_PathPosition > m_startPosition)
        {
            m_manager.m_IsStart = true;
        }
    }

}
