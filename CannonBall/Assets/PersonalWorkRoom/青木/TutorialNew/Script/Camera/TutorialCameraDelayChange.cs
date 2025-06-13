using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialCameraDelayChange : MonoBehaviour
{
    [SerializeField, CustomLabel("���炷����(�b)")]
    float m_delayTime = 1.0f;

    [SerializeField, CustomLabel("�J�ڌ��̃J����")]
    CinemachineVirtualCamera m_CVCamera;

    [SerializeField, CustomLabel("�J�ڐ�̃J����")]
    CinemachineVirtualCamera m_nextCVCamera;

    private async void Change()
    {
        await Task.Delay((int)(1000 * m_delayTime));

        // �J������ύX
        m_CVCamera.Priority = 0;
        m_nextCVCamera.Priority = 1;
    }


    private void Start()
    {

    }

    public void ChangeCamera()
    {
        Change();
    }
}
