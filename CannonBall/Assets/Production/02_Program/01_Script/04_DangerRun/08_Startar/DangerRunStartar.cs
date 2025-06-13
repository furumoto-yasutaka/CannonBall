using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerRunStartar : MonoBehaviour
{
    [SerializeField, CustomLabel("レディゴーコールバック")]
    private ReadyGoAnimationCallback m_readygo;

    [SerializeField, CustomLabel("カメラ移動コンポーネント")]
    private DangerRun_CameraMove m_cameraMove;

    [SerializeField, CustomLabel("プレイヤーコントローラー")]
    private PlayerController_DangerRun[] m_playerController;


    private void Update()
    {
        if (m_readygo.m_IsFinish)
        {
            m_cameraMove.StartMove();

            foreach (PlayerController_DangerRun p in m_playerController)
            {
                p.Active();
            }

            Destroy(this);
        }
    }
}
