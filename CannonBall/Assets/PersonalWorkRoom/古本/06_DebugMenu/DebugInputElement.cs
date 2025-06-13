/*******************************************************************************
*
*	タイトル：	入力受付要素の基底クラス(デバッグ用)
*	ファイル：	DebugInputElement.cs
*	作成者：	古本 泰隆
*	制作日：    2024/01/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DebugInputElement : MonoBehaviour
{
    [SerializeField, CustomLabel("インプットアクションマップ")]
    private NewInputActionMap m_inputAcitonMap;


    public NewInputActionMap GetActionMap()
    {
        return m_inputAcitonMap;
    }


    protected virtual void Awake()
    {
        m_inputAcitonMap.Init();
        m_inputAcitonMap.Enable();
    }

    protected virtual void Update()
    {
        m_inputAcitonMap.Update();
    }
}
