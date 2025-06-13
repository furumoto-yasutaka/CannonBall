using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCameraAroundMove : MonoBehaviour
{
    [SerializeField]
    float m_speed = 1.0f;

    CinemachineTrackedDolly m_trackedDolly;

    private void Start()
    {
        m_trackedDolly = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
    }


    private void Update()
    {
        m_trackedDolly.m_PathPosition += m_speed * Time.deltaTime;
    }


}
