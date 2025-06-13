/*******************************************************************************
*
*	�^�C�g���F	���ۓ��͏��X�N���v�g
*	�t�@�C���F	NewInputAction.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/21
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
