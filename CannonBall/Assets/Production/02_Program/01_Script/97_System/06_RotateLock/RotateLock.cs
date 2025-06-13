/*******************************************************************************
*
*	タイトル：	回転固定スクリプト
*	ファイル：	RotateLock.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLock : MonoBehaviour
{
    void Update()
    {
        // 回転を打ち消す
        transform.rotation = Quaternion.identity;
    }
}
