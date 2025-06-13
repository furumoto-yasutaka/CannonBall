/*******************************************************************************
*
*	タイトル：	抽象入力情報スクリプト
*	ファイル：	NewInputAction.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NewInputAction
{
    [SerializeField]
    protected string m_actionName = "";

    public string m_ActionName { get { return m_actionName; } }


    public abstract void Init();

    public abstract void Update();

    public abstract void Reset();

    protected abstract void Update_InputReset(int deviceId, int i);
}
