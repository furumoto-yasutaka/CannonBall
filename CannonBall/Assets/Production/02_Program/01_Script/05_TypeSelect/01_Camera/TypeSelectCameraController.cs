using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TypeSelectCameraController : MonoBehaviour
{
    [SerializeField, CustomLabel("シネマシーンブレイン")]
    private CinemachineBrain m_brain;

    private int m_currentCameraNum = 0;

    private List<CinemachineVirtualCamera> m_virtualCameras = new List<CinemachineVirtualCamera>();


    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_virtualCameras.Add(transform.GetChild(i).GetComponent<CinemachineVirtualCamera>());
        }
    }

    private void Update()
    {
        if (!m_brain.IsBlending)
        {
            ChangeCamera();
        }
    }

    public void ChangeCamera()
    {
        m_virtualCameras[m_currentCameraNum].Priority = 0;
        m_currentCameraNum = (m_currentCameraNum + 1) % m_virtualCameras.Count;
        m_virtualCameras[m_currentCameraNum].Priority = 1;
    }
}
