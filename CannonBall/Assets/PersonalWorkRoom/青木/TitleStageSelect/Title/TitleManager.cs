using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField, CustomLabel("デバック用")]
    bool m_isTitleFinish = false;


    [SerializeField]
    Animator m_CameraAnimator;



    public void SetTitleFinish(bool _isTitleFinish) {  m_isTitleFinish= _isTitleFinish; }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }

    private void Start()
    {

        //m_CameraAnimator = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Animator>();
    }


    private void Update()
    {
        if (m_isTitleFinish)
        {
            // タイトルシーンの最後の最後に行う処理
            TitleStageSelectManager.Instance.ExchangeStageSelect(TitleStageSelectManager.Scene.Tutorial);
            TitleTutorialView.Instance.View();

            m_isTitleFinish = false;

            m_CameraAnimator.SetInteger("State", (int)StageChangeManager.StageType.FightGame);
        }
    }







}
