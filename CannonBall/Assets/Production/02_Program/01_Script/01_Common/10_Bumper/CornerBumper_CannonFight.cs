/*******************************************************************************
*
*	タイトル：	バンパーのコリジョンイベントスクリプト
*	ファイル：	CornerBumper_CannonFight.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerBumper_CannonFight : CornerBumper
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Ball") ||
            collision.collider.CompareTag("PlayerCloneBody"))
        {
            collision.transform.root.GetComponent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

            m_animator.SetTrigger("IsBounce");
            CreateContactEffect();
            PlayContactSound();
        }
    }
}
