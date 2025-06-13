using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveResultSceneChangeToResult_DangerRun : MonoBehaviour
{
    SceneChanger m_Changer;

    bool m_isTrigger = false;

    private void Start()
    {
        m_Changer = GetComponent<SceneChanger>();
    }


    private void Update()
    {
        if (GameSetController_DangerRun.Instance.m_IsFinish && !m_isTrigger)
        {
            TemporaryDataLine.Instance.SetResultData();

            m_Changer.StartSceneChange(SceneNameEnum.Result);

            m_isTrigger = true;
        }
    }
}
