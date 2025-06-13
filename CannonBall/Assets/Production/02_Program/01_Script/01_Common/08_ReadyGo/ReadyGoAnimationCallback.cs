using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyGoAnimationCallback : SingletonMonoBehaviour<ReadyGoAnimationCallback>
{
    [SerializeField, CustomLabel("ReadyGo�G�t�F�N�g")]
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
            "�����J�n_���f�B",
            false);
    }

    public void PlayGoSe()
    {
        AudioManager.Instance.PlaySe(
            "�����J�n_�S�[",
            false);
    }
}
