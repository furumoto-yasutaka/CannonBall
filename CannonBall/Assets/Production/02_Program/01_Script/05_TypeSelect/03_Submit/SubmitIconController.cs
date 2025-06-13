using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitIconController : MonoBehaviour
{
    [SerializeField, CustomLabel("���莞�G�t�F�N�g�̐e�I�u�W�F�N�g")]
    private Transform m_submitEffectParent;

    private Animator m_animator;

    private bool m_isSubmit = false;

    public bool m_IsSubmit { get { return m_isSubmit; } }


    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Submit(int playerNum)
    {
        if (m_isSubmit) { return; }

        m_animator.SetBool("IsShow", true);
        m_isSubmit = true;
        EffectContainer.Instance.EffectPlay(
            "�^�C�v�I������_" + (playerNum + 1) + "P",
            m_submitEffectParent.position,
            m_submitEffectParent.rotation,
            m_submitEffectParent);
    }

    public void Cancel()
    {
        if (!m_isSubmit) { return; }

        m_animator.SetBool("IsShow", false);
        m_isSubmit = false;
    }
}
