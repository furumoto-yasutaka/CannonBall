/*******************************************************************************
*
*	タイトル：	プレイヤーの足のコリジョンイベントスクリプト(大乱闘モードver)
*	ファイル：	PlayerLegOnCollision_CannonFight.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision_CannonFight : PlayerLegOnCollision
{
    /// <summary> プレイヤーポイントコンポーネント </summary>
    [SerializeField, CustomLabel("プレイヤーポイントコンポーネント")]
    private PlayerPoint_CannonFight m_playerPoint;

    /// <summary> プレイヤー必殺技ポイントコンポーネント </summary>
    [SerializeField, CustomLabel("プレイヤー必殺技ポイントコンポーネント")]
    private PlayerSpMove m_playerSpMovePoint;


    /// <summary> プレイヤーが触れた際のイベント </summary>
    /// <param name="other"> プレイヤーのコリジョン </param>
    protected override void OnTriggerEnter_Player(Collider2D other)
    {
        base.OnTriggerEnter_Player(other);

        other.transform.root.GetComponent<PlayerPoint_CannonFight>().RequestKickMark(m_playerPoint);

        // 必殺技ポイント増加
        other.transform.root.GetComponent<PlayerSpMove>().AccumulateBeKickedPattern();
        m_playerSpMovePoint.AccumulateKickPattern();
    }

    /// <summary> 地形が触れた際のイベント </summary>
    /// <param name="other"> 地形のコリジョン </param>
    protected override void OnTriggerEnter_Platform(Collider2D other)
    {
        //// 足の角度と当たった地形の法線の方向が離れている場合蹴ったと判断しない
        //List<ContactPoint2D> points = new List<ContactPoint2D>();
        //other.GetContacts(points);
        //foreach (ContactPoint2D point in points)
        //{
        //    if (Mathf.Abs(Vector2.Dot(point.normal, m_playerController.m_KickDir)) <= 0.05f)
        //    {
        //        return;
        //    }
        //}

        base.OnTriggerEnter_Platform(other);
    }
}
