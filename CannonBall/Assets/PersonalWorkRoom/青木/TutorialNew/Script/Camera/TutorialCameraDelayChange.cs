using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialCameraDelayChange : MonoBehaviour
{
    [SerializeField, CustomLabel("‚¸‚ç‚·ŠÔ(•b)")]
    float m_delayTime = 1.0f;

    [SerializeField, CustomLabel("‘JˆÚŒ³‚ÌƒJƒƒ‰")]
    CinemachineVirtualCamera m_CVCamera;

    [SerializeField, CustomLabel("‘JˆÚæ‚ÌƒJƒƒ‰")]
    CinemachineVirtualCamera m_nextCVCamera;

    private async void Change()
    {
        await Task.Delay((int)(1000 * m_delayTime));

        // ƒJƒƒ‰‚ğ•ÏX
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
