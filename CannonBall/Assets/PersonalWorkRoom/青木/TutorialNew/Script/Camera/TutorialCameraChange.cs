using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TutorialCameraChange : MonoBehaviour
{
    [SerializeField, CustomLabel("遷移元のカメラ")]
    CinemachineVirtualCamera m_CVCamera;


    [SerializeField, CustomLabel("遷移先のカメラ")]
    CinemachineVirtualCamera m_nextCVCamera;

    private void Start()
    {
        
    }

    public void ChangeCamera()
    {
        // カメラを変更
        m_CVCamera.Priority = 0;
        m_nextCVCamera.Priority = 1;
    }
}
