using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TutorialCameraChange : MonoBehaviour
{
    [SerializeField, CustomLabel("�J�ڌ��̃J����")]
    CinemachineVirtualCamera m_CVCamera;


    [SerializeField, CustomLabel("�J�ڐ�̃J����")]
    CinemachineVirtualCamera m_nextCVCamera;

    private void Start()
    {
        
    }

    public void ChangeCamera()
    {
        // �J������ύX
        m_CVCamera.Priority = 0;
        m_nextCVCamera.Priority = 1;
    }
}
