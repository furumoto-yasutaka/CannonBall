/*******************************************************************************
*
*	タイトル：	プレイヤーId保持スクリプト
*	ファイル：	PlayerId.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/18
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerId : MonoBehaviour
{
    [SerializeField, CustomLabelReadOnly("プレイヤーID")]
    private int m_id;

    public int m_Id { get { return m_id; } }

    public void InitId(int id)
    {
        m_id = id;
    }
}
