using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DetailWindowNextSceneAlpha : MonoBehaviour
{
    [SerializeField]
    float m_speed = 4.0f;

    CanvasGroup m_canvasGroup;

    float m_alpha = 0.0f;

    bool m_isStart = false;

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();


        ResultSceneController.Instance.m_State.Subscribe(_ =>
        {
            if (_ == ResultSceneController.STATE.NEXT_SCENE_SELECT)
            {
                m_isStart = true;
            }
        }).AddTo(this);
    }


    private void Update()
    {
        if (m_isStart)
        {
            m_alpha += m_speed * Time.deltaTime;

            m_canvasGroup.alpha = m_alpha;

            if (m_alpha >= 1.0f)
            {
                m_alpha = 1.0f;
                m_canvasGroup.alpha = m_alpha;
                m_isStart = false;
            }
        }
    }
}
