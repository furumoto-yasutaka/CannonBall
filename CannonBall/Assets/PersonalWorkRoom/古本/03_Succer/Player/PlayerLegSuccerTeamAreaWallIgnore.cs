/*******************************************************************************
*
*	タイトル：	プレイヤーの足と他チームの境界壁との当たり判定を無視させるスクリプト
*	ファイル：	PlayerLegSuccerTeamAreaWallIgnore.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegSuccerTeamAreaWallIgnore : MonoBehaviour
{
    private List<Collider2D> m_anotherTeamAreaWallList = new List<Collider2D>();


    private void OnEnable()
    {
        foreach (Collider2D wallCol in m_anotherTeamAreaWallList)
        {
            Collider2D legCol = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(legCol, wallCol);
        }
    }

    /// <summary> 壁の初期化 </summary>
    /// <param name="wallParent"> 壁の親オブジェクト </param>
    /// <param name="teamColor"> どのチームか </param>
    public void InitWallList(Transform wallParent, SuccerTeamInfoSetter.TeamColor teamColor)
    {
        for (int i = 0; i < wallParent.childCount; i++)
        {
            if (i == (int)teamColor) { continue; }

            m_anotherTeamAreaWallList.Add(wallParent.GetChild(i).GetComponent<Collider2D>());
        }
    }
}
