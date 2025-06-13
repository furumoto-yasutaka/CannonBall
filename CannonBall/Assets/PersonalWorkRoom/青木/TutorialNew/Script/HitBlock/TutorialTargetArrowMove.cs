using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialTargetArrowMove : MonoBehaviour
{
    ITutorialHitBlock m_tutorialHitBlock;

    [SerializeField, CustomLabel("動き方")]
    AnimationCurve m_animationCurve;

    [SerializeField, CustomLabel("動く量")]
    float m_move = 1.0f;
    
    float m_time = 0f;


    private void Start()
    {
        // 取得
        m_tutorialHitBlock = transform.parent.GetComponent<ITutorialHitBlock>();

        // 終了処理
        m_tutorialHitBlock.GetIsSuccess().Subscribe(_=>
        {
            if (m_tutorialHitBlock.GetIsSuccess().Value == true )
            {
                // 初期座標に戻す
                transform.localPosition = Vector3.zero;

                // 以降処理する必要はないのでOFF
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
