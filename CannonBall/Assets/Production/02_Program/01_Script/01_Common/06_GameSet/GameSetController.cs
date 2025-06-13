using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetController : SingletonMonoBehaviour<GameSetController>
{
    private Animator m_animator;

    private bool m_isFinish = false;

    public bool m_IsFinish { get { return m_isFinish; } }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_animator.enabled)
        {
            if (Timer.Instance.m_TimeCounter <= 0.0f)
            {
                m_animator.enabled = true;
            }
        }
    }


    public void Finish()
    {
        m_isFinish = true;
    }

    public void PlayGameSetSe()
    {
        AudioManager.Instance.StopBgmAll();
        AudioManager.Instance.PlaySe(
            "試合終了_ゲームセット",
            false);
    }
}
