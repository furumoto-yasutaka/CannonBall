/*******************************************************************************
*
*	タイトル：	入力受付要素の基底クラス
*	ファイル：	InputElement.cs
*	作成者：	古本 泰隆
*	制作日：    2023/04/16
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputElement : MonoBehaviour
{
    /// <summary> マネージャー </summary>
    private InputGroupManager m_manager;

    /// <summary> 所属グループ </summary>
    private InputGroup m_group;

    /// <summary> 入力可能か </summary>
    private bool m_isCanInput = true;


    public NewInputActionMap GetActionMap()
    {
        return m_Group.m_Window.m_InputActionMap;
    }


    public InputGroupManager m_Manager
    {
        get { return m_manager; }
    }

    public InputGroup m_Group
    {
        get { return m_group; }
    }

    public bool m_IsCanInput
    {
        get { return m_group.m_IsCanInput && m_isCanInput; }
    }


    /// <summary>
    /// パラメータ初期化
    /// </summary>
    public virtual void InitializeParam(InputGroupManager manager, InputGroup group)
    {
        m_manager = manager;
        m_group = group;
    }

    /// <summary>
    /// 要素単位で入力をロック
    /// </summary>
    public virtual void LockInput()
    {
        m_isCanInput = false;
    }

    /// <summary>
    /// 要素単位で入力のロック解除
    /// </summary>
    public virtual void UnlockInput()
    {
        m_isCanInput = true;
    }
}
