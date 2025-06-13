/*******************************************************************************
*
*	タイトル：	ボール情報スクリプト
*	ファイル：	BallInfo.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfo : MonoBehaviour
{
    /// <summary> レアボールかどうか </summary>
    [SerializeField, CustomLabel("レアボールかどうか")]
    private bool m_isRareBall = false;


    public bool m_IsRareBall { get { return m_isRareBall; } }
}
