using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpController : SingletonMonoBehaviour<SpeedUpController>
{
    private Animator m_animator;


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Play()
    {
        m_animator.SetBool("IsShow", true);
    }

    public void AnimationDisabled()
    {
        m_animator.SetBool("IsShow", false);
    }
}
