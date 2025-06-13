using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last60second_CannonFightController : MonoBehaviour
{
    [SerializeField, CustomLabel("演出用エフェクト")]
    private ParticleSystem m_effect;

    private Animator m_animator;


    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_animator.enabled)
        {
            if (Timer.Instance.m_TimeCounter <= 60.0f)
            {
                m_animator.enabled = true;
            }
        }
    }

    public void PlaySe()
    {
        AudioManager.Instance.PlaySe("キャノンファイト_残り1分", false);
    }

    public void PlayEffect()
    {
        m_effect.Play();
    }
}
