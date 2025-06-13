/*******************************************************************************
*
*	タイトル：	プレイヤーが触れたら死亡するスクリプト
*	ファイル：	DeadZone.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : AliveZone
{
    protected override void OnTriggerExit2D(Collider2D collision) {}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            OnTriggerEnter_Player(collision);
        }
    }

    /// <summary> プレイヤーが触れた際のイベント </summary>
    /// <param name="collision"> プレイヤーのコリジョン </param>
    protected virtual void OnTriggerEnter_Player(Collider2D collision)
    {
        OnTriggerExit_Player(collision);
    }
}
