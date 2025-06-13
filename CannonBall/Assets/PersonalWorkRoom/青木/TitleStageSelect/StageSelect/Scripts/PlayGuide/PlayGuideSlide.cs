using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayGuideSlide : MonoBehaviour
{

    bool m_isHide = false;


    private void Start()
    {

        StageChangeEventTiming.Instance.m_isMove.Subscribe(v =>
        {
            if (v)
            {
                m_isHide = true;    
            }
        }
        ).AddTo(this);
    }


    private void Update()
    {
        if (m_isHide)
        {


        }
    }
}
