/*******************************************************************************
*
*	タイトル：	バンパーのコリジョンイベントスクリプト(デンジャラン用)
*	ファイル：	CornerBumper_DangerRun.cs
*	作成者：	古本 泰隆
*	制作日：    2024/01/14
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerBumper_DangerRun : CornerBumper
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("PlayerLeg"))
        {
            Transform root = collision.transform.root;
            if (!root.GetComponent<PlayerController>().m_BumperIgnore)
            {
                root.GetComponentInParent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

                m_animator.SetTrigger("IsBounce");
                CreateContactEffect();
                PlayContactSound();
                SetVibration(root.GetComponent<PlayerId>().m_Id);
            }
        }
    }
}
