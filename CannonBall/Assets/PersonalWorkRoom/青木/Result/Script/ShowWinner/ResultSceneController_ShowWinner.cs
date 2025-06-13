using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneController_ShowWinner : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera m_aroundVCam;

    [SerializeField]
    CinemachineVirtualCamera m_showWinnerVCam;

    [SerializeField]
    Image m_nextIconImage;


    private void Start()
    {
        ResultSceneController.Instance.m_State.Subscribe(v =>
        {
            if (v == ResultSceneController.STATE.SHOW_WINNER)
            {
                // ���փA�C�R�����N������
                m_nextIconImage.gameObject.SetActive(true);

                // �J������ύX
                m_aroundVCam.gameObject.SetActive(false);
                m_showWinnerVCam.gameObject.SetActive(true);

                ResultSceneController.Instance.SetWaitTime();

            }
        }).AddTo(this);

    }

    private void Update()
    {
    }

}
