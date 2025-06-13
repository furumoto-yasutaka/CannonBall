/*******************************************************************************
*
*	タイトル：	プレイヤーチーム情報設定スクリプト
*	ファイル：	SuccerTeamInfoSetter.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/01
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccerTeamInfoSetter : MonoBehaviour
{
    /// <summary> チームの色 </summary>
    public enum TeamColor
    {
        Red = 0,
        Blue,

        Length,
    }


    /// <summary> 各チームの配色 </summary>
    [SerializeField, CustomArrayLabel(typeof(TeamColor))]
    private Color[] m_teamUiColor;

    /// <summary> チームの移動範囲を制限する壁の親オブジェクト </summary>
    [SerializeField, CustomLabel("チームの移動範囲を制限する壁の親オブジェクト")]
    private Transform m_teamAreaWallParent;


    public Color[] m_TeamUiColor { get { return m_teamUiColor; } }

    public Transform m_TeamAreaWallParent { get { return m_teamAreaWallParent; } }


    // PlayerIdSetterにペアレント解除の役割があるので、
    // 早く実行されるよう優先度を-200にあげています
    private void Awake()
    {
        // チーム情報を付与
        // ★チーム選択画面がある場合ここで情報を取ってきて
        // プレイヤーにチーム情報を割り振り、プレイヤーの位置も自分の陣地の方に移動させる
        // とりあえず現状は存在するプレイヤーの半分を赤、青にするだけ
        int i = 0;
        for (; i < transform.childCount / 2; i++)
        {
            Transform t = transform.GetChild(i);
            t.GetComponent<SuccerTeamInfo>().InitInfo(TeamColor.Red, this);
        }
        for (; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.GetComponent<SuccerTeamInfo>().InitInfo(TeamColor.Blue, this);
        }
    }
}
