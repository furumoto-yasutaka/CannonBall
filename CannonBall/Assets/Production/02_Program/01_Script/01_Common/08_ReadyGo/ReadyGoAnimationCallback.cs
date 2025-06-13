using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyGoAnimationCallback : SingletonMonoBehaviour<ReadyGoAnimationCallback>
{
    [SerializeField, CustomLabel("ReadyGoエフェクト")]
    private ParticleSystem m_eff;

    private bool m_isFinish = false;

    public bool m_IsFinish { get { return m_isFinish; } }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }

    public void CreateEffect()
    {
        m_eff.Play();
    }

    public void Finish()
    {
        m_isFinish = true;
    }

    public void PlayReadySe()
    {
        AudioManager.Instance.PlaySe(
            "試合開始_レディ",
            false);
    }

    public void PlayGoSe()
    {
        AudioManager.Instance.PlaySe(
            "試合開始_ゴー",
            false);
    }
}
