using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpMove_Huge : PlayerSpMove
{
    /// <summary> �A�j���[�^�[�R���|�[�l���g </summary>
    private Animator m_animator;


    protected override void Start()
    {
        base.Start();

        m_animator = transform.GetComponent<Animator>();
    }

    /// <summary> �K�E�Z�𔭓����� </summary>
    public override void StartSpMove()
    {
        base.StartSpMove();

        m_animator.SetBool("IsHuge", true);
        AudioManager.Instance.PlaySe("�L���m���t�@�C�g_���剻", false);
    }

    /// <summary> �K�E�Z���I������ </summary>
    public override void EndSpMove()
    {
        base.EndSpMove();

        m_animator.SetBool("IsHuge", false);
    }
}
