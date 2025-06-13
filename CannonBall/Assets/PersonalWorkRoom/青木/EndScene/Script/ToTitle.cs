using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTitle : MonoBehaviour
{
    [SerializeField]
    float m_sceneChangeTime = 3.0f;

    float m_timeCount = 0.0f;

    bool m_isChange = false;

    SceneChanger m_sceneChanger;

    private void Start()
    {
        m_sceneChanger = GetComponent<SceneChanger>();
    }

    private void Update()
    {
        if (m_isChange)
        {
            return;
        }

        m_timeCount += Time.deltaTime;
        if (m_timeCount >= m_sceneChangeTime)
        {
            m_sceneChanger.StartSceneChange();

            m_isChange = true;
        }
    }
}
