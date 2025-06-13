/*******************************************************************************
*
*	タイトル：	プレイヤーのペアレント解除スクリプト
*	ファイル：	PlayerParentCanceller.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/01
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParentCanceller : MonoBehaviour
{
    private void Awake()
    {
        // ペアレント解除
        while (transform.childCount > 0)
        {
            transform.GetChild(0).parent = null;
        }

        // 不要なのでこのコンポーネントを削除
        Destroy(this);
    }
}
