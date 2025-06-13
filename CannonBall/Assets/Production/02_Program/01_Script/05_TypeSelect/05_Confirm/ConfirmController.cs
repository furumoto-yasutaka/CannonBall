using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmController : MonoBehaviour
{
    private Animator m_mainAnimator;

    [SerializeField, CustomLabel("にぎやかし演出用アニメーター")]
    private Animator m_livelyAnimator;

    [SerializeField, CustomLabel("最終確認決定エフェクトの親オブジェクト")]
    private Transform m_confirmSubmitEffParent;


    private void Start()
    {
        m_mainAnimator = GetComponent<Animator>();
    }

    public void Show()
    {
        m_mainAnimator.SetBool("IsShow", true);
        m_livelyAnimator.SetTrigger("Lively");
    }

    public void Hide()
    {
        m_mainAnimator.SetBool("IsShow", false);
        m_livelyAnimator.SetTrigger("Wait");
    }

    public void Submit()
    {
        m_mainAnimator.SetTrigger("Submit");
        EffectContainer.Instance.EffectPlay(
            "最終確認決定",
            m_confirmSubmitEffParent.position,
            m_confirmSubmitEffParent.rotation,
            m_confirmSubmitEffParent);
    }
}
