/*******************************************************************************
*
*	タイトル：	チュートリアルの文字送り
*	ファイル：	Tutorial_TextData.cs
*	作成者：	青木 大夢
*	制作日：    2023/10/09
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_TextData : MonoBehaviour
{
    [SerializeField]
    bool m = false;

    private void Update()
    {
        if (m)
        {
            GetComponent<TextFeed>().NextTextLine();

            m = false;
        }
    }

}
