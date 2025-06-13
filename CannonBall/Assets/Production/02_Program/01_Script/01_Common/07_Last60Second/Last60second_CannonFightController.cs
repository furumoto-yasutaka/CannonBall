using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last60second_CannonFightController : MonoBehaviour
{
    [SerializeField, CustomLabel("���o�p�G�t�F�N�g")]
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
        AudioManager.Instance.PlaySe("�L���m���t�@�C�g_�c��1��", false);
    }

    public void PlayEffect()
    {
        m_effect.Play();
    }
}
