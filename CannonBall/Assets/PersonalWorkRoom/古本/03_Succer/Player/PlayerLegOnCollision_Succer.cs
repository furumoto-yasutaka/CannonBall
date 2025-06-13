/*******************************************************************************
*
*	タイトル：	プレイヤーの足のコリジョンイベントスクリプト(サッカーモードver)
*	ファイル：	PlayerLegOnCollision_Succer.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision_Succer : PlayerLegOnCollision
{
    /// <summary> プレイヤー入力受付 </summary>
    private PlayerImpact_Succer m_playerImpact_Succer;


    protected override void Start()
    {
        base.Start();

        m_playerImpact_Succer = transform.root.GetComponent<PlayerImpact_Succer>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Ball"))
        {// ボールを蹴った
            OnTriggerEnter_Ball(other);
        }
    }

    /// <summary> ボールに触れた際のコリジョンイベント </summary>
    /// <param name="other"> ボールのコリジョン </param>
    private void OnTriggerEnter_Ball(Collider2D other)
    {
        m_playerImpact_Succer.KickBallRecoil();
        other.transform.root.GetComponent<BallImpact>().Kicked(
            m_playerController.m_KickDir,
            m_playerImpact_Succer.m_KickBallPower,
            m_rb.velocity);
    }
}
