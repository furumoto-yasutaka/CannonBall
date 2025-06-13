using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class PlayGuideViewLogo : MonoBehaviour
{
    enum STATE
    {
        NONE,
        VIEW,
        HIDE,
    }

    [SerializeField]
    private float m_speed = 1.0f;

    [SerializeField]
    private float m_delayTime = 1.0f;

    [SerializeField, CustomReadOnly]
    private STATE m_state = STATE.NONE;

    private Image m_image;

    private float m_clipValue = 0.0f;


    PlayGuideView m_playGuideView;


    private void OnEnable()
    {
        m_state = STATE.VIEW;
        m_clipValue = 0.0f;
    }

    private void OnDisable()
    {
        m_state = STATE.NONE;
        m_clipValue = 0.0f;
    }


    private void Start()
    {
        m_image = GetComponent<Image>();
        m_image.material.SetFloat("_ClipTime", m_clipValue);

        m_playGuideView = transform.parent.GetComponent<PlayGuideView>();

        {

            //StageChangeEventTiming.Instance.m_isMove.

            //StageChangeEventTiming.Instance.m_isMove.Delay(TimeSpan.FromSeconds(m_delayTime)).Subscribe(v =>
            //{
            //    m_state = !v ? STATE.VIEW : STATE.HIDE;
            //}
            //).AddTo(this);

            //Observable.Range(0, 1).Do(x => { }).Delay(TimeSpan.FromSeconds(m_delayTime)).Subscribe();
            //var observable = Observable.Start(() => "").DoOnSubscribe(() => Debug.Log("")) ;
            //Observable.Timer(System.TimeSpan.FromSeconds(1.0f)).SelectMany(observable).Subscribe();

            //// m_delayTime 秒　Viewステータスに移行する時間をずらす
            //Observable.Range(0, 1).Delay(TimeSpan.FromSeconds(m_delayTime)).Subscribe(x =>
            //{
            //    Debug.Log("いあまああああああああああああああああ");
            //});
        }

        StageChangeEventTiming.Instance.m_isMove.Subscribe(v =>
            {
                if (!v)
                {
                    m_state = STATE.VIEW;
                }
                else
                {
                    m_state = STATE.HIDE;
                }
            }
        ).AddTo(this);
    }



    private void Update()
    {
        if (StageChangeManager.Instance.GetCurrentStage() != transform.parent.GetComponent<PlayGuideId>().GetStageType())
        {
            if (m_state != STATE.NONE)
            {
                m_state = STATE.HIDE;
            }            
        }




        switch (m_state)
        {
            case STATE.NONE:
                break;
            case STATE.VIEW:
                DelayView();
                break;
            case STATE.HIDE:
                Hide();
                break;
            default:
                break;
        }
    }


    async void DelayView()
    {
        await Task.Delay((int)(m_delayTime * 1000));

        m_clipValue += m_speed * Time.deltaTime;
        m_image.material.SetFloat("_ClipTime", m_clipValue);

        if (m_clipValue >= 1.0f)
        {
            m_clipValue = 1.0f;
            m_state = STATE.NONE;
        }

    }


    void Hide()
    {
        m_clipValue -= m_speed * Time.deltaTime;
        m_image.material.SetFloat("_ClipTime", m_clipValue);


        if (m_clipValue <= 0.0f)
        {
            m_clipValue = 0.0f;

            m_state = STATE.NONE;

            m_playGuideView.SetLogoDesable();
        }
    }


}
