/*******************************************************************************
*
*	タイトル：	サッカーゴールのコリジョンイベントスクリプト
*	ファイル：	SuccerGoalGateOnCollition.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccerGoalGateOnCollition : MonoBehaviour
{
    /// <summary> 赤チーム側のゴールゲートかどうか </summary>
    [SerializeField, CustomLabel("赤チーム側のゴールゲートかどうか")]
    private bool m_isRedTeam = true;


    private void OnTriggerExit2D(Collider2D collision)
    {
        // ゴールゲートをちゃんとくぐっているか確認
        if (!IsGoal(collision.transform.position)) { return; }

        if (collision.CompareTag("PlayerBody"))
        {// プレイヤー
            OnTriggerExit_Player(collision);
        }
        else if (collision.CompareTag("Ball"))
        {// ボール
            OnTriggerExit_Ball(collision);
        }
    }

    /// <summary> ゴールしているかどうか判断 </summary>
    /// <param name="position"> ボールの座標 </param>
    private bool IsGoal(Vector3 position)
    {
        if (m_isRedTeam)
        {
            // ゴールゲートより外側だったらゴール
            if (transform.position.x > position.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // ゴールゲートより外側だったらゴール
            if (transform.position.x < position.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary> プレイヤーが離れた際のコリジョンイベント </summary>
    /// <param name="collision"> ボールの座標 </param>
    private void OnTriggerExit_Player(Collider2D collision)
    {
        // ★プレイヤーごとにどのチームに所属するか指定できるようにする場合ここを変更する
        int playerId = collision.transform.root.GetComponent<PlayerId>().m_Id;
        if (playerId < 2)
        {
            SuccerTeamPointManager.Instance.PlayerGoalIn_Blue();
        }
        else
        {
            SuccerTeamPointManager.Instance.PlayerGoalIn_Red();
        }
    }

    /// <summary> ボールが離れた際のコリジョンイベント </summary>
    /// <param name="collision"> ボールの座標 </param>
    private void OnTriggerExit_Ball(Collider2D collision)
    {
        if (!collision.transform.root.GetComponent<BallInfo>().m_IsRareBall)
        {// 通常ボール
            if (m_isRedTeam)
            {
                SuccerTeamPointManager.Instance.BallGoalIn_Blue();
            }
            else
            {
                SuccerTeamPointManager.Instance.BallGoalIn_Red();
            }
        }
        else
        {// レアボール
            if (m_isRedTeam)
            {
                SuccerTeamPointManager.Instance.RareBallGoalIn_Blue();
            }
            else
            {
                SuccerTeamPointManager.Instance.RareBallGoalIn_Red();
            }
        }
    }
}
