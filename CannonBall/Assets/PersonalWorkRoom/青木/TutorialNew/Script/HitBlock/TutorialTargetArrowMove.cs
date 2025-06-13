using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialTargetArrowMove : MonoBehaviour
{
    ITutorialHitBlock m_tutorialHitBlock;

    [SerializeField, CustomLabel("������")]
    AnimationCurve m_animationCurve;

    [SerializeField, CustomLabel("������")]
    float m_move = 1.0f;
    
    float m_time = 0f;


    private void Start()
    {
        // �擾
        m_tutorialHitBlock = transform.parent.GetComponent<ITutorialHitBlock>();

        // �I������
        m_tutorialHitBlock.GetIsSuccess().Subscribe(_=>
        {
            if (m_tutorialHitBlock.GetIsSuccess().Value == true )
            {
                // �������W�ɖ߂�
                transform.localPosition = Vector3.zero;

                // �ȍ~��������K�v�͂Ȃ��̂�OFF
                enabled = false;
            }
        }).AddTo(this);
    }


    private void Update()
    {
        float posx = m_animationCurve.Evaluate(m_time) * m_move;
        m_time += Time.deltaTime;

        transform.localPosition = Vector3.right * posx;
    }

}
