/*******************************************************************************
*
*	タイトル：	プレイヤーが生存しているか判断するスクリプト
*	ファイル：	AliveZone.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/18
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveZone : MonoBehaviour
{
    private bool m_isRun = true;


    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_isRun) { return; }

        if (collision.CompareTag("PlayerBody"))
        {
            OnTriggerExit_Player(collision);
        }
    }

    protected virtual void OnTriggerExit_Player(Collider2D collision)
    {
        // 場外に出たのでリスポーンリストに追加
        Transform trans = collision.transform.root;
        // プレイヤーの死亡処理を行う
        if (RespawnManager.CheckInstance())
        {
            trans.GetComponent<PlayerController>().Death(RespawnManager.Instance.m_RevivalInterval);
            // RespawnManagerに追加依頼する
            RespawnManager.Instance.AddRespawnPlayer(trans);
        }
    }

    private void OnApplicationQuit()
    {
        m_isRun = false;
    }
}
