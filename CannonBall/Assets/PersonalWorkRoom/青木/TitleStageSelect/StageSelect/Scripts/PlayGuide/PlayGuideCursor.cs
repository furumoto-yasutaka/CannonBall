using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;

public class PlayGuideCursor : MonoBehaviour
{
    enum State
    {
        NONE,
        VIEW_IN,
        VIEW_OUT,
    }



    [SerializeField]
    GameObject m_CommnonSlide;

    [SerializeField]
    PlayGuideViewLogo m_ViewLogo;

    [SerializeField]
    PlayGuideViewSlide m_ViewSlide;


    Animator m_animator;

    State m_state = State.NONE;


    private void Start()
    {
        m_animator = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Animator>();





        StageChangeEventTiming.Instance.m_isMove.Subscribe(v =>
        {
            m_state = !v ? State.VIEW_IN : State.VIEW_OUT;
        }
        ).AddTo(this);
    }

    private void SlideHide()
    {

    }
    void Data()
    {
        // �Ȃ�̃X�e�[�W�I������Ă�H

        // �������̒���Lnegth�ǂ̂��炢�H�z��ԍ���
        // �X�e�[�W���S������̂�Length�������OK�̂͂�
    }


    bool GuideViewTiming()
    {
        //Debug.Log("m_TrackedDolly.m_PathPosition" + m_TrackedDolly.m_PathPosition);


        //float pos = m_TrackedDolly.m_PathPosition;
        //float nowPos = Mathf.Floor(m_TrackedDolly.m_PathPosition);

        //Debug.Log("pos" + pos);

        //if (pos >= m_ViewTiming - nowPos)
        //{
        //    Debug.Log("1");
        //}
        //if (nowPos + m_ViewTiming >= pos)
        //{
        //    Debug.Log("2");
        //}



        //if (pos >= nowPos - m_ViewTiming && nowPos + m_ViewTiming >= pos)
        //{
        //    return true;
        //}

        return false;
    }




}
