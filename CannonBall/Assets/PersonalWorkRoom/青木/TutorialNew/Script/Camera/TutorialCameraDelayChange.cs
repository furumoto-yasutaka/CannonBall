using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialCameraDelayChange : MonoBehaviour
{
    [SerializeField, CustomLabel("ずらす時間(秒)")]
    float m_delayTime = 1.0f;

    [SerializeField, CustomLabel("遷移元のカメラ")]
    CinemachineVirtualCamera m_CVCamera;

    [SerializeField, CustomLabel("遷移先のカメラ")]
    CinemachineVirtualCamera m_nextCVCamera;

    private async void Change()
    {
        await Task.Delay((int)(1000 * m_delayTime));

        // カメラを変更
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
