using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ResultSceneController_AroundCamera : MonoBehaviour
{
    [SerializeField]
    FadeDuringResult m_fadeDuringResult;

    [SerializeField]
    float m_resultTitleShowTime = 3.0f;

    float m_timeCount = 0;

    bool m_isStart = true;


    private void Start()
    {
        ResultSceneController.Instance.m_State.Subscribe(v =>
        {
            if (v == ResultSceneController.STATE.RESULT_TITLE)
            {
                m_isStart = true;
            }
        }).AddTo(this);

    }


    private void Update()
    {
        if (m_isStart) 
        {
            m_timeCount += Time.deltaTime;

            if (m_timeCount >= m_resultTitleShowTime)
            {
                // �{���̓t�F�[�h��ɍs������
                m_fadeDuringResult.SetFadeOutStart();

                m_isStart = false;
            }
        }

    }
}
