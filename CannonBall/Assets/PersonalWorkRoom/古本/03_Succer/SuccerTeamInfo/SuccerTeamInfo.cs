/*******************************************************************************
*
*	タイトル：	プレイヤーチーム情報スクリプト
*	ファイル：	SuccerTeamInfo.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/01
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccerTeamInfo : MonoBehaviour
{
    /// <summary> どのチームか </summary>
    [SerializeField, CustomLabelReadOnly("どのチームか")]
    private SuccerTeamInfoSetter.TeamColor m_teamColor = SuccerTeamInfoSetter.TeamColor.Red;

    /// <summary> プレイヤー番号UI </summary>
    [SerializeField, CustomLabel("プレイヤー番号UI")]
    private SpriteRenderer m_playerNumberUi;

    /// <summary> 体の当たり判定 </summary>
    [SerializeField, CustomLabel("体の当たり判定")]
    private Collider2D m_bodyCol;

    /// <summary> 足の当たり判定 </summary>
    [SerializeField, CustomLabel("足の当たり判定")]
    private Collider2D m_legCol;


    public SuccerTeamInfoSetter.TeamColor m_TeamColor { get { return m_teamColor; } }


    /// <summary> 得点設定 </summary>
    /// <param name="color"> どのチームか </param>
    /// <param name="setter"> サッカーチーム設定コンポーネント </param>
    public void InitInfo(SuccerTeamInfoSetter.TeamColor color, SuccerTeamInfoSetter setter)
    {
        m_teamColor = color;

        // UIの色を変更する
        m_playerNumberUi.color = setter.m_TeamUiColor[(int)color];

        // 自分のチーム以外の壁との当たり判定を削除する
        // 体
        for (int i = 0; i < setter.m_TeamAreaWallParent.childCount; i++)
        {
            if (i == (int)color) { continue; }

            Transform child = setter.m_TeamAreaWallParent.GetChild(i);
            BoxCollider2D wallCol = child.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(m_bodyCol, wallCol);
        }
        // 足
        m_legCol.transform.GetComponent<PlayerLegSuccerTeamAreaWallIgnore>().InitWallList(
            setter.m_TeamAreaWallParent,
            m_TeamColor);
    }
}
