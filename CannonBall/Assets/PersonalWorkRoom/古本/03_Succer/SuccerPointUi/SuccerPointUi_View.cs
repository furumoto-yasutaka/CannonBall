/*******************************************************************************
*
*	タイトル：	サッカーの得点表示制御スクリプト
*	ファイル：	SuccerPointUi_View.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuccerPointUi_View : MonoBehaviour
{
    /// <summary> 得点設定 </summary>
    /// <param name="value"> 設定する値 </param>
    public void SetValue(int value)
    {
        TextMeshPro text = transform.GetChild(0).GetComponent<TextMeshPro>();
        text.text = value.ToString();
    }
}
