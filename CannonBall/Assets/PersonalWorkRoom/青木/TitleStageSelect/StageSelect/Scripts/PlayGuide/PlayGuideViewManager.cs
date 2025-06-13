using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

// ハイド状態に遷移する用のマネージャー
public class PlayGuideViewManager : SingletonMonoBehaviour<PlayGuideViewManager>
{
    [SerializeField]
    GameObject[] m_Slides;

    PlayGuideView[] m_playGuideViews;


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_playGuideViews = new PlayGuideView[m_Slides.Length];
        for (int i = 0; i < m_playGuideViews.Length; i++)
        {
            m_playGuideViews[i] = m_Slides[i].GetComponent<PlayGuideView>();
        }
    }

    private void Start()
    {
        StageChangeEventTiming.Instance.m_isMove.Subscribe(v =>
        {
            Observable
                .NextFrame()
                .Subscribe(_ =>
                {
                    if (!v)
                    {
                        m_Slides[(int)StageChangeManager.Instance.GetCurrentStage() - 1].SetActive(true);
                    }
                    else
                    {
                        m_playGuideViews[(int)StageChangeManager.Instance.GetCurrentStage() - 1].SetHide();
                    }

                });
        }
        ).AddTo(this);

    }
}
