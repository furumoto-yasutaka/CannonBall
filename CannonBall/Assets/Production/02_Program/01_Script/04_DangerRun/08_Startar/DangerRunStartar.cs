using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerRunStartar : MonoBehaviour
{
    [SerializeField, CustomLabel("���f�B�S�[�R�[���o�b�N")]
    private ReadyGoAnimationCallback m_readygo;

    [SerializeField, CustomLabel("�J�����ړ��R���|�[�l���g")]
    private DangerRun_CameraMove m_cameraMove;

    [SerializeField, CustomLabel("�v���C���[�R���g���[���[")]
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
