using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteResultScene : MonoBehaviour
{
    [SerializeField]
    float m_sceneChangeTime = 10.0f;

    SceneChanger m_Changer;

    float m_timeCount = 0.0f;

    bool m_isFin = false;

    private void Start()
    {
        m_Changer = GetComponent<SceneChanger>();
    }

    private void Update()
    {
        if (m_isFin)
        {
            return;
        }

        m_timeCount += Time.deltaTime;
        if (m_timeCount >= m_sceneChangeTime)
        {
            m_Changer.StartSceneChange(SceneNameEnum.Result);

            TemporaryDataLine.Instance.SetResultData();

            m_isFin = true;
        }
    }


}
