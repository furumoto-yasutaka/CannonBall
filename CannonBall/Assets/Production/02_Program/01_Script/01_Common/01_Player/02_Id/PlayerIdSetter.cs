/*******************************************************************************
*
*	タイトル：	プレイヤーID設定スクリプト
*	ファイル：	PlayerIdSetter.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/18
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdSetter : MonoBehaviour
{
    private void Awake()
    {
        // プレイヤーにIDを付与
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<PlayerId>().InitId(i);
        }

        // 不要なのでこのコンポーネントを削除
        Destroy(this);
    }
}
