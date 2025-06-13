/*******************************************************************************
*
*	タイトル：	プレイヤーが生存しているか判断するスクリプト(サッカーモードver)
*	ファイル：	AliveZone_Succer.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveZone_Succer : AliveZone
{
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (collision.CompareTag("Ball"))
        {
            OnTriggerExit_Ball(collision);
        }
    }

    /// <summary> ボールが離れた際のコリジョンイベント </summary>
    /// <param name="collision"> ボールのコリジョン </param>
    private void OnTriggerExit_Ball(Collider2D collision)
    {
        // 場外に出たのでボールマネージャーから削除依頼
        Transform trans = collision.transform.root;
        BallManager.Instance.DestroyBall(trans.gameObject);
    }
}
