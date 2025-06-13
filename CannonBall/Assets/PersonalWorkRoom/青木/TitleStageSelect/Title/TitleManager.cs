using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField, CustomLabel("�f�o�b�N�p")]
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
            // �^�C�g���V�[���̍Ō�̍Ō�ɍs������
            TitleStageSelectManager.Instance.ExchangeStageSelect(TitleStageSelectManager.Scene.Tutorial);
            TitleTutorialView.Instance.View();

            m_isTitleFinish = false;

            m_CameraAnimator.SetInteger("State", (int)StageChangeManager.StageType.FightGame);
        }
    }







}
