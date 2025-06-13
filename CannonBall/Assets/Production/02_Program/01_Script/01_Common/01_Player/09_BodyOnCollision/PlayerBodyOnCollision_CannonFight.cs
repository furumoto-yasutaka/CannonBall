/*******************************************************************************
*
*	タイトル：	プレイヤーの体のコリジョンイベントスクリプト(大乱闘モードver)
*	ファイル：	PlayerBodyOnCollision_CannonFight.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyOnCollision_CannonFight : PlayerBodyOnCollision
{
    public struct ImpactEffectInfo
    {
        public string EffectName;
        public float Threshold;
    }


    [SerializeField, CustomLabel("プレイヤーポイントコンポーネント")]
    private PlayerPoint_CannonFight m_playerPoint;

    [SerializeField, CustomArrayLabel(new string[] { "小", "中", "大", })]
    private ImpactEffectInfo[] m_effectInfo;


    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnCollisionExit_Player(collision);
        }
    }

    /// <summary> プレイヤーが離れた際のイベント </summary>
    /// <param name="collision"> プレイヤーのコリジョン </param>
    private void OnCollisionExit_Player(Collision2D collision)
    {
        // 敵プレイヤーに自分をマークするようリクエストする
        m_playerPoint.RequestContactMark(
            collision.transform.root.GetComponent<PlayerPoint_CannonFight>(),
            collision.transform.GetComponent<Rigidbody2D>().velocity);
    }
}
