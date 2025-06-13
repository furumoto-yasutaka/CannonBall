using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpMove_Huge : PlayerSpMove
{
    /// <summary> アニメーターコンポーネント </summary>
    private Animator m_animator;


    protected override void Start()
    {
        base.Start();

        m_animator = transform.GetComponent<Animator>();
    }

    /// <summary> 必殺技を発動する </summary>
    public override void StartSpMove()
    {
        base.StartSpMove();

        m_animator.SetBool("IsHuge", true);
        AudioManager.Instance.PlaySe("キャノンファイト_巨大化", false);
    }

    /// <summary> 必殺技を終了する </summary>
    public override void EndSpMove()
    {
        base.EndSpMove();

        m_animator.SetBool("IsHuge", false);
    }
}
